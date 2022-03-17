using Mapster;
using MyRestaurant.BusinessLogic.Interfaces;
using MyRestaurant.BusinessLogic.Models;
using MyRestaurant.DataAccess.Interface;
using System.Linq;

namespace MyRestaurant.BusinessLogic.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileService (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int CreateProfile(UserProfileModel user)
        {
            var destObject = user.Adapt<MyRestaurant.DataAccess.Models.User>();
            _unitOfWork.User.Add(destObject);
            _unitOfWork.Save();
            return destObject.Id;
        }
        public UserProfileModel SearchUser(UserProfileModel model)
        {
            var destObject = model.Adapt<MyRestaurant.DataAccess.Models.User>();
            var user = _unitOfWork.UserProfile.Get(x => x.Address == model.Address && x.Name == model.Name).FirstOrDefault();
            UserProfileModel result = null;
            if (user != null)
            {
                result = model;
            }
            return result;
        }
        
        public bool PhoneVerifaction(string phoneNumber)
        {
            var user = _unitOfWork.User.Get(x => x.PhoneNumber == phoneNumber).FirstOrDefault();
            bool result;
            if (user != null)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

    }
}
