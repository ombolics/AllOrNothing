using AllOrNothing.Data;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AllOrNothing.Mapping
{
    [AutoMap(typeof(Player), ReverseMap = true)]
    public class PlayerDto : ObservableRecipient
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
        private int _id;
        public int Id 
        {
            get => _id;
            set => SetProperty(ref _id, value); 
        }
        private string _name;
        public string Name 
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _institute;
        public string Institute 
        {
            get => _institute;
            set => SetProperty(ref _institute, value);
        }
        private string _nickName;
        public string NickName 
        {
            get => _nickName;
            set => SetProperty(ref _nickName, value);
        }
        private bool _isDeleted;
        public bool IsDeleted 
        {
            get => _isDeleted;
            set => SetProperty(ref _isDeleted, value);
        }

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
