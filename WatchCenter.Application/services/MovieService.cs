


namespace WatchCenter.Application.services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFormFileService _fileService;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IFormFileService fileService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<Result<ContentDto>> AddMovieAsync(AddMovieDto dto)
        {
            var content = await _unitOfWork.Contents.GetByIdAsync(dto.ContentId,x=>x.Movie);
            if (content is null)
                return Result<ContentDto>.Failure("content not found");

            if((int)content.Type!=0)
                return Result<ContentDto>.Failure("this content not a movie");

            if(content.Movie!=null)
                return Result<ContentDto>.Failure("content already have movie");

            var movieUrl= await _fileService.SaveFileAsync(dto.Movie, "Movies");
            if (movieUrl is null)
                return Result<ContentDto>.Failure("field to save movie file");
            var movie = new Movie()
            {
                ContentId = dto.ContentId,
                MovieUrl = movieUrl
            };
            

            await _unitOfWork.Movies.AddAsync(movie);


            var result = await _unitOfWork.SaveAsync();
            if(!result)
                return Result<ContentDto>.Failure("field to save in db");

            var contentUpdated = await _unitOfWork.Contents.GetByIdAsync(dto.ContentId, x => x.Movie);

            var contentdto = _mapper.Map<ContentDto>(contentUpdated);

            return Result<ContentDto>.Success(contentdto);

        }

        public async Task<Result<ICollection<ContentDto>>> GetAllAsync()
        {
            var result = await _unitOfWork.Contents.GetAllAsync();
            result = result.Where(c=> (int)c.Type==0);
            var contentdto = _mapper.Map<ICollection<ContentDto>>(result);

            return Result<ICollection<ContentDto>>.Success(contentdto);
        }

        public async Task<Result<MovieDto>> GetByIdAsync(string id)
        {
            // get movie frm db
            var movie = await _unitOfWork.Contents.GetByIdAsync(id,m=>m.Movie);

            // check if movie null
            if (movie is null)
                return Result<MovieDto>.Failure("Movie not found");

            var  movieDto = _mapper.Map<MovieDto>(movie);

            return Result<MovieDto>.Success(movieDto);

        }
    }
}
