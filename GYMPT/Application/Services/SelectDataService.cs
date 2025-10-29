<<<<<<< HEAD
﻿using GYMPT.Application.Facades;
=======
﻿using System;
using System.Linq;
>>>>>>> Modulo_Membership
using GYMPT.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceMembership.Application.Interfaces;

namespace GYMPT.Application.Services
{
    public class SelectDataService : ISelectDataService
    {
<<<<<<< HEAD
        private readonly ISelectDataFacade _facade;

        public SelectDataService(ISelectDataFacade facade)
        {
            _facade = facade;
=======
        private readonly IUserService _userService;
        private readonly IMembershipService _membershipService;
        private readonly IDisciplineService _disciplineService;

        public SelectDataService(IUserService userService, IMembershipService membershipService, IDisciplineService disciplineService)
        {
            _userService = userService;
            _membershipService = membershipService;
            _disciplineService = disciplineService;
>>>>>>> Modulo_Membership
        }

        public async Task<SelectList> GetUserOptionsAsync()
        {
<<<<<<< HEAD
            // Ahora representa los CLIENTES (antes "users")
            return await _facade.GetClientOptionsAsync();
=======
            var users = await _userService.GetAllUsers();
            var userOptions = users
                .Where(u => u.Role == "Client")
                .Select(u => new
                {
                    u.Id,
                    FullName = $"{u.Name} {u.FirstLastname}"
                });
            return new SelectList(userOptions, "Id", "FullName");
>>>>>>> Modulo_Membership
        }

        public async Task<SelectList> GetMembershipOptionsAsync()
        {
<<<<<<< HEAD
            return await _facade.GetMembershipOptionsAsync();
=======
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
>>>>>>> Modulo_Membership
        }

        public async Task<SelectList> GetInstructorOptionsAsync()
        {
<<<<<<< HEAD
            return await _facade.GetInstructorOptionsAsync();
=======
            var users = await _userService.GetAllUsers();
            var instructors = users
                .Where(u => u.Role == "Instructor")
                .Select(u => new
                {
                    Id = (long)u.Id,
                    FullName = $"{u.Name} {u.FirstLastname}"
                });
            return new SelectList(instructors, "Id", "FullName");
>>>>>>> Modulo_Membership
        }
    }
}
