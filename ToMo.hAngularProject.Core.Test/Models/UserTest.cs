using ToMo.hAngularProject.Core.Models;
using Xunit;

namespace ToMo.hAngularProject.Core.Test.Models
{
    public class UserTest
    {
        private readonly User _user;

        public UserTest()
        {
            _user = new User();
        }

        [Fact]
        public void User_CanBeInitialized()
        {
            Assert.NotNull(_user);
        }
        
        [Fact]
        public void User_Id_MustBeInt()
        {
            
            Assert.True(_user.Id is int);
        }

        [Fact]
        public void User_SetId_StoredID()
        {
            _user.Id = 1;
            Assert.Equal(1, _user.Id);
        }
        
        [Fact]
        public void User_UpdateId_StoredID()
        {
            _user.Id = 1;
            _user.Id = 2;
            Assert.Equal(2, _user.Id);
        }

        [Fact]
        public void User_SetName_StoreNameAsString()
        {
            _user.Name = "Item";
            Assert.Equal("Item", _user.Name);
        }
    }
}