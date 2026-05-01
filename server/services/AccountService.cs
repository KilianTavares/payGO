using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using server.interfaces;
using server.models;
using server.models.responses;
using server.database;


namespace server.services
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public AccountService(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(AccountLoginDTO request)
        {
            // Find account by email
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == request.Email);
            if (account == null || string.IsNullOrEmpty(account.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Verify password against stored hash
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHash);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Generate JWT token
            var token = GenerateJwtToken(account);
            return token;
        }

        public Task LogoutAsync()
        {
            // JWT tokens are stateless, so logout is typically handled client-side
            // by removing the token from storage. Server-side blacklisting could be
            // implemented here if needed.
            return Task.CompletedTask;
        }

        public async Task RegisterAsync(AccountRegistrationDTO request)
        {
            if (await _context.Accounts.AnyAsync(a => a.Email == request.Email))
            {
                throw new InvalidOperationException("Email already exists.");
            }

            // Hash the password using BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12);

            var account = new Account
            {
                Name = request.Email,
                PasswordHash = passwordHash,
                Email = request.Email,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task<GetAccountResponse> GetAccountAsync(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }
            var response = new GetAccountResponse
            {
                Id = account.Id,
                Name = account.Name,
                Email = account.Email,
                Address = account.Address,
                PhoneNumber = account.PhoneNumber,
            };
            return response;
        }

        private string GenerateJwtToken(Account account)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, account.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}