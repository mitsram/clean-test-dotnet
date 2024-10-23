using System;
using System.Text.Json;
using System.Threading.Tasks;
using PetApi.Application.Repositories;
using PetApi.Domain.Entities;

namespace PetApi.Application.UseCases;

public class PetUseCases
{
    private readonly IPetRepository _petRepository;

    public PetUseCases(IPetRepository petRepository)
    {
        _petRepository = petRepository;
    }

    public async Task<Pet> AddPetAsync(Pet pet)
    {
        Console.WriteLine($"Adding pet: {JsonSerializer.Serialize(pet)}");
        var result = await _petRepository.AddPetAsync(pet);
        Console.WriteLine($"Added pet: {JsonSerializer.Serialize(result)}");
        return result;
    }

    public async Task<Pet> GetPetByIdAsync(long petId)
    {
        Console.WriteLine($"Getting pet with ID: {petId}");
        var result = await _petRepository.GetPetByIdAsync(petId);
        Console.WriteLine($"Retrieved pet: {JsonSerializer.Serialize(result)}");
        return result;
    }

    public async Task<Pet> UpdatePetAsync(Pet pet)
    {
        Console.WriteLine($"Updating pet: {JsonSerializer.Serialize(pet)}");
        var result = await _petRepository.UpdatePetAsync(pet);
        Console.WriteLine($"Updated pet: {JsonSerializer.Serialize(result)}");
        return result;
    }

    public async Task<bool> DeletePetAsync(long petId)
    {
        Console.WriteLine($"Deleting pet with ID: {petId}");
        var result = await _petRepository.DeletePetAsync(petId);
        Console.WriteLine($"Delete result: {result}");
        return result;
    }
}
