﻿using AllOrNothing.Data;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AllOrNothing.AutoMapper.Dto
{
    [AutoMap(typeof(Question), ReverseMap = true)]
    public class QuestionDto : ObservableRecipient
    {
        public int Id { get; set; }
        public QuestionResourceType ResourceType { get; set; }
        public QuestionType Type { get; set; }
        public byte[] Resource { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public int Value { get; set; }

    }
}
