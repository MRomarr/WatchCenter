

using AutoMapper;

namespace WatchCenter.Application.services
{
    internal class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<GenreDto>>> GetAllGenresAsync()
        {
            // get genre
            var result = await _unitOfWork.Genres.GetAllAsync();

            // convert to DTOs
            var resultDto = _mapper.Map<IEnumerable<GenreDto>>(result);

            //return result
            return new Result<IEnumerable<GenreDto>>()
            {
                Succeeded = true,
                Data = resultDto
            };
        }

        public async Task<Result<IEnumerable<ContentDto>>> GetContentsByGenreIdAsync(string id)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(id, g => g.Contents,g=>g.Contents);
            if (genre is null)
            {
                return new Result<IEnumerable<ContentDto>>
                {
                    Succeeded = false,
                    Message = "Genre not found."
                };
            }
            
            // Convert contents to DTOs
            var contentDtos = _mapper.Map<IEnumerable<ContentDto>>(genre.Contents);
            
            // Return success result
            return new Result<IEnumerable<ContentDto>>
            {
                Succeeded = true,
                Data = contentDtos
            };
        }

        public async Task<Result<GenreDto>> CreateGenreAsync(string GenreName)
        {
            // Validate the genre name
            var genres = await _unitOfWork.Genres.GetAllAsync();
            if (genres.Any(g => g.Name.Equals(GenreName, StringComparison.OrdinalIgnoreCase)))
            {
                return new Result<GenreDto>
                {
                    Succeeded = false,
                    Message = "Genre already exists."
                };
            }

            // Create a new genre
            var genre = new Genre
            {
                Name = GenreName
            };

            // Add the genre to the database
            await _unitOfWork.Genres.AddAsync(genre);
            var result =await _unitOfWork.SaveAsync();
            if (!result)
            {
                return new Result<GenreDto>
                {
                    Succeeded = false,
                    Message = "Failed to create genre."
                };
            }

            // Convert to DTO
            var genreDto = _mapper.Map<GenreDto>(genre);

            // Return success result
            return new Result<GenreDto>
            {
                Succeeded = true,
                Data = genreDto,
            };
        }

        public async Task<Result<GenreDto>> UpdateGenreAsync(UpdateGenreDto dto)
        {
            // get genre
            var genre = await _unitOfWork.Genres.GetByIdAsync(dto.Id);

            //check genre
            if (genre is null)
            {
                return new Result<GenreDto>
                {
                    Succeeded = false,
                    Message = "Genre not found."
                };
            }

            // check if genre name already exists
            var genres = await _unitOfWork.Genres.GetAllAsync();
            if (genres.Any(g => g.Name.Equals(dto.NewName, StringComparison.OrdinalIgnoreCase) && g.Id != dto.Id))
            {
                return new Result<GenreDto>
                {
                    Succeeded = false,
                    Message = "Genre name already exists."
                };
            }

            // update genre
            genre.Name = dto.NewName;
            _unitOfWork.Genres.Update(genre);
            var result = await _unitOfWork.SaveAsync();
            if (!result)
            {
                return new Result<GenreDto>
                {
                    Succeeded = false,
                    Message = "Failed to update genre."
                };
            }

            // Convert to DTO
            var genreDto = _mapper.Map<GenreDto>(genre);
            
            // Return success result
            return new Result<GenreDto>
            {
                Succeeded = true,
                Data = genreDto,
                Message = "Genre updated successfully."
            };
        }

        public async Task<Result> DeleteGenreAsync(string id)
        {
            // get genre
            var genre = await _unitOfWork.Genres.GetByIdAsync(id);

            // check genre
            if (genre is null)
            {
                return new Result
                {
                    Succeeded = false,
                    Message = "Genre not found."
                };
            }

            // delete genre
            _unitOfWork.Genres.Delete(genre);
            var result = await _unitOfWork.SaveAsync();
            if (!result)
            {
                return new Result
                {
                    Succeeded = false,
                    Message = "Failed to delete genre."
                };
            }

            // Return success result
            return new Result
            {
                Succeeded = true,
                Message = "Genre deleted successfully."
            };
        }

        
    }
}
