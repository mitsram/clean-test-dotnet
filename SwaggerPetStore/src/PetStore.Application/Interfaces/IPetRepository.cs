using PetStore.Domain.Entities;

namespace PetStore.Application.Interfaces;

public interface IPetRepository
{
    Task<Pet> AddPetAsync(Pet pet);
    Task<Pet> GetPetByIdAsync(long petId);
    Task<Pet> UpdatePetAsync(Pet pet);
    Task<bool> DeletePetAsync(long petId);
}

