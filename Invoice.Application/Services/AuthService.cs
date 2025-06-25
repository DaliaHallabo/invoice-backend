using Invoice.Application.DTOs.Auth;
using Invoice.Application.Interfaces;
using Invoice.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtToken _jwtToken;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtToken jwtToken)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtToken = jwtToken;
    }

    public async Task<string> RegisterAsync(RegisterDto model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            Role = model.Role
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Registration failed: {errors}");
        }

        await _userManager.AddToRoleAsync(user, model.Role);

        return _jwtToken.CreateTokenAsync(user);
    }

    public async Task<string> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) throw new Exception("Invalid email or password");

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded) throw new Exception("Invalid email or password");

        return _jwtToken.CreateTokenAsync(user);
    }
}