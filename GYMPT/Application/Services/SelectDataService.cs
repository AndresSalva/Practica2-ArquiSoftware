<<<<<<< HEAD
using System;
using System.Linq;
=======
﻿using GYMPT.Application.Facades;
>>>>>>> Service-Usuario
using GYMPT.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceClient.Application.Interfaces;
using ServiceDiscipline.Application.Interfaces;
using ServiceMembership.Application.Interfaces;

namespace GYMPT.Application.Services
{
    public class SelectDataService : ISelectDataService
    {
<<<<<<< HEAD
        // Este IUserService ahora apunta correctamente a ServiceClient.Application.Interfaces.IUserService
        private readonly IUserService _userService;
        // Este IMembershipService apunta correctamente a GYMPT.Application.Interfaces.IMembershipService
        private readonly IMembershipService _membershipService;
        private readonly IDisciplineService _disciplineService;

        public SelectDataService(IUserService userService, IMembershipService membershipService, IDisciplineService disciplineService)
        {
            _userService = userService;
            _membershipService = membershipService;
            _disciplineService = disciplineService;
=======
        private readonly ISelectDataFacade _facade;

        public SelectDataService(ISelectDataFacade facade)
        {
            _facade = facade;
>>>>>>> Service-Usuario
        }

        public async Task<SelectList> GetUserOptionsAsync()
        {
<<<<<<< HEAD
            // --- CAMBIO 2: Usar el nombre de método correcto del nuevo contrato ---
            var users = await _userService.GetAllAsync(); // El método ahora se llama GetAllAsync

            var userOptions = users
                .Where(u => u.Role == "Client")
                .Select(u => new
                {
                    u.Id,
                    FullName = $"{u.Name} {u.FirstLastname}"
                });
            return new SelectList(userOptions, "Id", "FullName");
=======
            // Ahora representa los CLIENTES (antes "users")
            return await _facade.GetClientOptionsAsync();
>>>>>>> Service-Usuario
        }

        public async Task<SelectList> GetMembershipOptionsAsync()
        {
<<<<<<< HEAD
            var membershipResult = await _membershipService.GetAllMemberships();
            if (membershipResult.IsFailure || membershipResult.Value is null)
            {
                throw new InvalidOperationException(membershipResult.Error ?? "No se pudo obtener la lista de membresías.");
            }

            return new SelectList(membershipResult.Value, "Id", "Name");
        }

        public async Task<SelectList> GetDisciplineOptionsAsync()
        {
            var disciplines = await _disciplineService.GetAllDisciplines();
            return new SelectList(disciplines, "Id", "Name");
=======
            return await _facade.GetMembershipOptionsAsync();
>>>>>>> Service-Usuario
        }

        public async Task<SelectList> GetInstructorOptionsAsync()
        {
<<<<<<< HEAD
            // --- CAMBIO 2 (Repetido): Usar el nombre de método correcto del nuevo contrato ---
            var users = await _userService.GetAllAsync(); // El método ahora se llama GetAllAsync

            var instructors = users
                .Where(u => u.Role == "Instructor")
                .Select(u => new
                {
                    Id = (long)u.Id,
                    FullName = $"{u.Name} {u.FirstLastname}"
                });
            return new SelectList(instructors, "Id", "FullName");
=======
            return await _facade.GetInstructorOptionsAsync();
>>>>>>> Service-Usuario
        }
    }
}
