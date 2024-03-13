using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProductService.Data.DataAccess;
using ProductService.Data.Dto;
using ProductService.Data.Entities;
using System.Web;

namespace Products.Services.Core
{
    public interface IColorService
    {
        public Task<ResultModel> Post(string name, string image);
        public Task<ResultModel> GetByIds(List<Guid> ids);
    }
    public class ColorService : IColorService
    {
        readonly ProductDbContext _db;
        public ResultModel _result;

        public ColorService(ProductDbContext db)
        {
            _db = db;
            _result = new ResultModel();
        }

        public async Task<ResultModel> GetByIds(List<Guid> ids)
        {
            try
            {
                List<Color> colors = new List<Color>();
                ids.ForEach(id =>
                {
                    var color = _db.Color.AsQueryable().First(c => c.Id == id);
                    color.Image = HttpUtility.UrlDecode(color.Image);
                    colors.Add(color);
                });

                _result.Data = colors;
                _result.IsSuccess = true;
                _result.Message = "Get Color Successful";
            }
            catch (Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }
            return _result;
        }

        public async Task<ResultModel> Post(string name, string image)
        {
            try
            {
                Color color = new Color() { Name = name, Image = image };

                _db.Color.InsertOneAsync(color);

                _result.IsSuccess = true;

                _result.Message = "Post Color Successful";
            }
            catch (Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }
            return _result;

        }

    }
}
