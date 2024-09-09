using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using SM.Core.Common.Constants;
using SM.Core.Domain.Dtos;
using SM.Core.Domain.Entities;
using SM.Core.Exceptions;
using SM.Core.Extensions;
using SM.Core.Services.EntityChangeServices;
using SM.Core.Services.EntityChangeServices.Model;
using SM.Core.Utilities.Security.Jwt;

namespace SM.Core.DataAccess.Contexts
{
    public class ProjectDbContext : DbContext, IProjectContext
    {
        private readonly ITokenHelper _tokenHelper;
        protected readonly IConfiguration _configuration;
        private readonly ICapPublisher _capPublisher;
        private readonly IEntityChangeServices _entityChangeServices;

        public ProjectDbContext(ITokenHelper tokenHelper, IConfiguration configuration, ICapPublisher capPublisher, IEntityChangeServices entityChangeServices)
        {
            _tokenHelper = tokenHelper;
            _configuration = configuration;
            _capPublisher = capPublisher;
            _entityChangeServices = entityChangeServices;
        }

        public bool IsDataTransferMode { get; set; }
        public bool PublishCompleted => _publishCompleted;
        private bool _publishCompleted = true;

        protected DbContextOptions _options;
        public DbContextOptions Options
        {
            get
            {
                return _options;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreContext")));
            }
            //optionsBuilder.UseLowerCaseNamingConvention();
            _options = optionsBuilder.Options;
        }

        private void SetDefaultVal(List<EntityEntry> changeTrack)
        {
            foreach (var entry in changeTrack)
            {
                if (!(entry.Entity is IEntityDefault))
                {
                    continue;
                }

                var track = entry.Entity as IEntityDefault;

                var userId = _tokenHelper.GetUserIdByCurrentToken();
                if (entry.State is EntityState.Added)
                {
                    track.InsertTime = DateTime.UtcNow;
                    track.InsertUserId = userId;
                    if (!IsDataTransferMode)
                    {
                        track.UpdateTime = DateTime.UtcNow;
                        track.UpdateUserId = userId;
                    }
                }
                if (entry.State is EntityState.Modified)
                {
                    track.UpdateTime = DateTime.UtcNow;
                    track.UpdateUserId = userId;
                }
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var changeTrack = this.ChangeTracker.Entries()
                .Where(c => c.State is EntityState.Added | c.State is EntityState.Modified | c.State is EntityState.Deleted)
                .ToList();
            ChangeTrackControl(changeTrack);
            SetDefaultVal(changeTrack);
            var changeTrackStates = ChangeTrackStateCalculate(changeTrack);
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            PublishData(changeTrack, changeTrackStates);
            return result;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            var changeTrack = this.ChangeTracker.Entries()
                .Where(c => c.State is EntityState.Added | c.State is EntityState.Modified | c.State is EntityState.Deleted)
                .ToList();
            ChangeTrackControl(changeTrack);
            SetDefaultVal(changeTrack);
            var changeTrackStates = ChangeTrackStateCalculate(changeTrack);
            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            result.ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    PublishData(changeTrack, changeTrackStates);
                }
            }, cancellationToken).ConfigureAwait(true);

            return result;
        }

        private void ChangeTrackControl(List<EntityEntry> changeTrack)
        {
            if (!(this is ISubscribeDbContext))
            {
                if (changeTrack.Any(a => a.Entity is IReadOnlyEntity))
                {
                    throw new CustomAppException(Messages.ThisEntityCannotEditOnThisMicroservice);
                }
                _publishCompleted = !changeTrack.Any(a => a.Entity is IPublishEntity);
            }
        }

        private List<ChangeTrackState> ChangeTrackStateCalculate(List<EntityEntry> changeTrack)
        {
            var result = new List<ChangeTrackState>();
            if (changeTrack.Any(a => a.Entity is IPublishEntity))
            {
                var i = 0;

                foreach (var itemChange in changeTrack)
                {
                    result.Add(new ChangeTrackState
                    {
                        Index = i,
                        State = itemChange.State
                    });
                    i++;
                }
            }
            return result;
        }

        private void PublishData(List<EntityEntry> changeTrack, List<ChangeTrackState> changeTrackStates)
        {
            if (!changeTrackStates.Any())
            {
                return;
            }
            try
            {
                var hasTransaction = this.Database.CurrentTransaction != null;
                if (hasTransaction)
                {
                    var changeTrackCopy = new EntityEntry[changeTrack.Count];
                    changeTrack.CopyTo(changeTrackCopy);
                    _entityChangeServices.ChangeEntriesList.Add(new ChangeEntityModel
                    {
                        ChangeTrack = changeTrackCopy,
                        ChangeTrackStates = changeTrackStates
                    });
                    _publishCompleted = true;
                    return;
                }

                var i = 0;

                foreach (var itemChange in changeTrack)
                {
                    if (itemChange.Entity is IPublishEntity)
                    {
                        var processName = ((IPublishEntity)itemChange.Entity).GeneratePublishName(changeTrackStates[i].State);
                        try
                        {
                            _capPublisher.Publish(processName, itemChange.Entity);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                    i++;
                }
                _publishCompleted = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }
    }
}
