using MelkYab.Backend.Data.Dtos;
using MelkYab.Backend.Data.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MelkYab.Backend.Data.Tables
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}