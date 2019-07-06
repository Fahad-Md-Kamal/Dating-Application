using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApplication.Helpers;
using DatingApplication.Models;

namespace DatingApplication.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id, bool isCurrentUser);
         Task<Photo> GetPhoto(int Id);
         Task<Photo> GetMainPhotoForUser(int userId);
         Task<Like> GetLike(int UserId, int recipentId);
         Task<Message> GetMessage(int Id);
         Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
         Task<IEnumerable<Message>> GetMessageThread(int UserId, int recipentId); 
    }
}