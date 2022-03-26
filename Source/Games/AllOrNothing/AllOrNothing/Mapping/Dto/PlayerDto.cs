using AllOrNothing.Data;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using CommunityToolkit.Mvvm.Input;

namespace AllOrNothing.Mapping
{
    [AutoMap(typeof(Player), ReverseMap = true)]
    public class PlayerDto
    {
        public PlayerDto()
        {

        }
        public PlayerDto(PlayerDto dto)
        {
            if (dto != null)
            {
                Id = dto.Id;
                Name = dto.Name;
                Institute = dto.Institute;
                NickName = dto.NickName;
                IsDeleted = dto.IsDeleted;
            }        
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Institute { get; set; }
        public string NickName { get; set; }
        public bool IsDeleted { get; set; }

        // public ObservableCollection<PlayerDto> OriginalTeam { get; set; }

        //RelayCommand because interface types's serialisation is not allowed
        [Ignore]

        public RelayCommand<object> RemoveCommand { get; set; }

        public override string ToString()
        {
            return Name;
        }

        internal bool HasTheSameValue(PlayerDto dto)
        {
            return Id == dto.Id &&
                Name == dto.Name &&
                Institute == dto.Institute &&
                NickName == dto.NickName &&
                IsDeleted == dto.IsDeleted;
        }
    }
}
