using System;
using System.Linq;
using System.Threading.Tasks;
using Geev.Authorization.Users;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.Runtime.Session;
using Geev.ZeroCore.SampleApp.Core;
using Shouldly;
using Xunit;

namespace Geev.Zero.Users
{
    public class UserManager_Tokens_Tests : GeevZeroTestBase
    {
        private readonly GeevUserManager<Role, User> _geevUserManager;
        private readonly IRepository<UserToken, long> _userTokenRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserManager_Tokens_Tests()
        {
            _geevUserManager = Resolve<GeevUserManager<Role, User>>();
            _userTokenRepository = Resolve<IRepository<UserToken, long>>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Should_Add_TokenValidityKey()
        {
            using (var uow = Resolve<IUnitOfWorkManager>().Begin())
            {
                var user = await _geevUserManager.GetUserByIdAsync(GeevSession.GetUserId());
                var tokenValidityKey = Guid.NewGuid().ToString();
                await _geevUserManager.AddTokenValidityKeyAsync(user, tokenValidityKey, DateTime.UtcNow.AddDays(1));
                var isTokenValidityKeyValid = await _geevUserManager.IsTokenValidityKeyValidAsync(user, tokenValidityKey);

                isTokenValidityKeyValid.ShouldBeTrue();
            }
        }

        [Fact]
        public async Task Should_Not_Valid_Expired_TokenValidityKey()
        {
            using (var uow = Resolve<IUnitOfWorkManager>().Begin())
            {
                var user = await _geevUserManager.GetUserByIdAsync(GeevSession.GetUserId());
                var tokenValidityKey = Guid.NewGuid().ToString();
                await _geevUserManager.AddTokenValidityKeyAsync(user, tokenValidityKey, DateTime.UtcNow);
                var isTokenValidityKeyValid = await _geevUserManager.IsTokenValidityKeyValidAsync(user, tokenValidityKey);

                isTokenValidityKeyValid.ShouldBeFalse();
            }
        }

        [Fact]
        public async Task Should_Remove_Expired_TokenValidityKeys()
        {
            using (_unitOfWorkManager.Begin())
            {
                var user = await _geevUserManager.GetUserByIdAsync(GeevSession.GetUserId());

                await _geevUserManager.AddTokenValidityKeyAsync(user, Guid.NewGuid().ToString(), DateTime.UtcNow);
                await _geevUserManager.AddTokenValidityKeyAsync(user, Guid.NewGuid().ToString(), DateTime.UtcNow.AddDays(1));
                await _geevUserManager.AddTokenValidityKeyAsync(user, Guid.NewGuid().ToString(), DateTime.UtcNow.AddDays(1));
                _unitOfWorkManager.Current.SaveChanges();

                var allTokens = _userTokenRepository.GetAllList(t => t.UserId == user.Id);
                allTokens.Count.ShouldBe(3);
            }

            using (_unitOfWorkManager.Begin())
            {
                var user = await _geevUserManager.GetUserByIdAsync(GeevSession.GetUserId());

                await _geevUserManager.IsTokenValidityKeyValidAsync(user, Guid.NewGuid().ToString());
                _unitOfWorkManager.Current.SaveChanges();

                var allTokens = _userTokenRepository.GetAllList(t => t.UserId == user.Id);
                allTokens.Count.ShouldBe(2);
            }
        }

        [Fact]
        public async Task Should_Remove_Given_Name_TokenValidityKey()
        {
            var tokenValidityKey = Guid.NewGuid().ToString();

            using (_unitOfWorkManager.Begin())
            {
                var user = await _geevUserManager.GetUserByIdAsync(GeevSession.GetUserId());

                await _geevUserManager.AddTokenValidityKeyAsync(user, tokenValidityKey, DateTime.UtcNow.AddDays(1));
                _unitOfWorkManager.Current.SaveChanges();

                var allTokens = _userTokenRepository.GetAllList(t => t.UserId == user.Id);
                allTokens.Count.ShouldBe(1);
            }

            using (_unitOfWorkManager.Begin())
            {
                var user = await _geevUserManager.GetUserByIdAsync(GeevSession.GetUserId());

                await _geevUserManager.RemoveTokenValidityKeyAsync(user, tokenValidityKey);
                _unitOfWorkManager.Current.SaveChanges();

                var allTokens = _userTokenRepository.GetAllList(t => t.UserId == user.Id);
                allTokens.Count.ShouldBe(0);
            }
        }
    }
}
