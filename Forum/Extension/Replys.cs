//using Forum.DAL.Context;
//using Forum.DAL.Entities;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//namespace System.Collections.Generic
//{
//    public static IEnumerable<dynamic> GetReplys<T>(this IEnumerable<T> source)
//    {
//        return source.Where(R => (R as Article)!.StatusJournal == null ||
//                                 (R as Article)!.StatusJournal!
//                                 .OrderByDescending(S => S.OperationDate)
//                                 .FirstOrDefault() == null ||
//                                 (R as Article)!.StatusJournal!
//                                 .OrderByDescending(S => S.OperationDate)
//                                 .FirstOrDefault()!.IsDeleted != true)
//                     .Select(R => new
//                     {
//                         Id = (R as Article)!.Id,
//                         Text = (R as Article)!.Text,
//                         AuthorId = (R as Article)!.AuthorId,
//                         TopicId = (R as Article)!.TopicId,
//                         Author = (R as Article)!.Author! == null ? null : new
//                         {
//                             ID = (R as Article)!.Author!.ID,
//                             Login = (R as Article)!.Author!.Login,
//                             RealName = (R as Article)!.Author!.RealName,
//                             Avatar = (R as Article)!.Author!.Avatar,
//                             Email = (R as Article)!.Author!.Email
//                         },
//                         StatusJournal = (R as Article)!.StatusJournal == null ? null :
//                                         (R as Article)!.StatusJournal!
//                                         .OrderByDescending(S => S.OperationDate)
//                                         .FirstOrDefault()!,
//                         CreatedDate = (R as Article)!.CreatedDate,
//                         ReplyId = (R as Article)!.ReplyId,
//                         Replys = (R as Article)!.Replys == null ? null :
//                                  (R as Article)!.Replys!.GetReplys()
//                     });
//    }
//}
