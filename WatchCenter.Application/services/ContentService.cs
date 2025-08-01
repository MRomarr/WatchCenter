
namespace WatchCenter.Application.services
{
    internal class ContentService : IContentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFormFileService _formFileService;
        private readonly IMapper _mapper;

        public ContentService(IUnitOfWork unitOfWork, IMapper mapper, IFormFileService formFileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _formFileService = formFileService;
        }

        public async Task<Result<ContentDto>> GetByIdAsync(string id)
        {
            // get content 
            var content = await _unitOfWork.Contents.GetByIdAsync(id);

            // check if content null
            if (content == null)
            {
                return Result<ContentDto>.Failure("Content not found.");
            }

            // map content to ContentDto
            var dto = _mapper.Map<ContentDto>(content);

            return Result<ContentDto>.Success(dto);
        }
        public async Task<Result<IEnumerable<ContentDto>>> SearchAsync(string query,int Page,int PageSize)
        {
            var contents = await _unitOfWork.Contents.GetAllAsync(
                c => c.Movie,
                c => c.Series,
                c => c.Series.Seasons);

            contents = contents
                .Where(c =>c.Title.Contains(query, StringComparison.OrdinalIgnoreCase) 
                ||c.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Skip((Page-1)* PageSize).Take(PageSize).ToList();

            if (contents == null || !contents.Any())
            {
                return Result<IEnumerable<ContentDto>>.Failure("No contents found.");
            }

            // map contents to ContentDto
            var dtos = _mapper.Map<IEnumerable<ContentDto>>(contents);

            // return result
            return Result<IEnumerable<ContentDto>>.Success(dtos);


        }
        public async Task<Result<ContentDto>> CreateAsync(string UserId, CreateContentDto dto)
        {
            var contents = await _unitOfWork.Contents.GetAllAsync();

            // check if content with same title already exists
            if (contents.Any(c => c.Title.Equals(dto.Title, StringComparison.OrdinalIgnoreCase)
            &&(int)c.Type==dto.Type))
            {
                return Result<ContentDto>.Failure("Content with the same title already exists.");
            }

            //check if genre exist
            if (await _unitOfWork.Genres.GetByIdAsync(dto.GenreId) is null)
                return Result<ContentDto>.Failure("Genre Not Found");

            // map CreateContentDto to Content
            var content = _mapper.Map<Content>(dto);
            content.UserId = UserId;
            if (dto.Trailer is not null)
            {
                var trailerUrl = await _formFileService.SaveFileAsync(dto.Trailer,"Trailer");
                if (trailerUrl != null)
                {
                    content.TrailerUrl = trailerUrl;
                }
            }
            if (dto.Poster is not null)
            {
                var posterUrl = await _formFileService.SaveFileAsync(dto.Poster, "Posters");
                if (posterUrl != null)
                {
                    content.PosterUrl = posterUrl;
                }
            }
            if (dto.Type == 1)
            {
                var series = new Series()
                {
                    ContentId = content.Id
                };
            }

            // add content to repository
            await _unitOfWork.Contents.AddAsync(content);

            
            // save changes
            var  result = await _unitOfWork.SaveAsync();
            if (!result)
            {
                return Result<ContentDto>.Failure("Failed to create content.");
            }
          
            // map content to ContentDto
            var contentDto = _mapper.Map<ContentDto>(content);
          
            // return result
            return Result<ContentDto>.Success(contentDto);


        }
        public async Task<Result<ContentDto>> UpdateAsync(UpdateContentDto dto)
        {
            var content = await _unitOfWork.Contents.GetByIdAsync(dto.ContentId);
            if (content is null)
                return Result<ContentDto>.Failure("content not found");

            var contents = await _unitOfWork.Contents.GetAllAsync();
            if(contents.Any(c=>c.Title==dto.Title&&c.Id!=dto.ContentId))
                return Result<ContentDto>.Failure($"this name already exist");

            var Data = _mapper.Map(dto, content);

            _unitOfWork.Contents.Update(Data);

            var result = await _unitOfWork.SaveAsync();
            if (!result)
            {
                return Result<ContentDto>.Failure("Failed to update content.");
            }

            // map content to ContentDto
            var contentDto = _mapper.Map<ContentDto>(content);

            // return result
            return Result<ContentDto>.Success(contentDto);



        }
        public async Task<Result> DeleteAsync(string id)
        {
            var content = await _unitOfWork.Contents.GetByIdAsync(id,c=>c.Movie);
            if (content is null)
                return Result.Failure("content not found");
            
            _unitOfWork.Contents.Delete(content);

            var result = await _unitOfWork.SaveAsync();
            if(!result)
                return Result.Failure("field to delete");

            return Result.Success("deleted sccesfuly");

        }
    }
}
