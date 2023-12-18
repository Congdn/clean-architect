﻿using Default.Application.Interfaces.Common;
using Default.Application.Interfaces.Logger;
using Default.Application.Interfaces.UnitOfWorks;
using Default.Domain.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Defaults.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly ICurrentPrincipal _currentPrincipal;
        private bool disposed = false;
        protected readonly ICustomLogger<IUnitOfWork> _logger;
        private PrincipalModel _principal = null;

        public UnitOfWork(TContext context, IHttpContextAccessor httpContextAccessor, ICustomLogger<IUnitOfWork> logger,
            ICurrentPrincipal currentPrincipal)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _currentPrincipal = currentPrincipal;
            _logger = logger;
            _principal = _currentPrincipal.Principal;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose the db context.
                    _context.Dispose();
                }
            }

            disposed = true;
        }

        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false)
        {
            //if (ensureAutoHistory)
            //{
            //    _context.EnsureAutoHistory();
            //}
            var currentDetectChangesSetting = _context.ChangeTracker.AutoDetectChangesEnabled;

            try
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
                _context.ChangeTracker.DetectChanges();

                var modifiedEntries = _context.ChangeTracker.Entries<AuditedEntity>()
                    .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted);

                AuditEntity(modifiedEntries);

                _context.ChangeTracker.DetectChanges();

                return await _context.SaveChangesAsync();
            }
            finally
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = currentDetectChangesSetting;
            }
        }

        private void AuditEntity(IEnumerable<EntityEntry<Default.Application.Interfaces.Common.AuditedEntity>> modifiedEntries)
        {
            var now = DateTime.Now;
            var userId = _principal != null ? _principal.UserId : Guid.Empty;
            foreach (var entry in modifiedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy = userId.ToString();
                     //   AuditUpdatingField(entry);

                        _logger.LogInfo("Added entity {@Entity} by user {UserId}"); //;, entry.Entity, userId);
                        break;

                    case EntityState.Modified:
                        //   AuditUpdatingField(entry);

                        _logger.LogInfo("Updated entity to {@Entity} by user {UserId}"); //, entry.Entity, userId);
                        break;

                    case EntityState.Deleted:
                        _logger.LogInfo("Deleted entity {@Entity} by user {UserId}");//, entry.Entity, userId);
                        break;
                }
            }

            //void AuditUpdatingField(EntityEntry<Default.Application.Interfaces.Common.IAuditedEntity> entry)
            //{
            //    entry.Entity.UpdatedAt = now;
            //    entry.Entity.UpdatedBy = userId;
            //}
        }
    }
}
