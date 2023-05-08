using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAutoresCLase.Data;
using WebApiAutoresCLase.DTOs;
using WebApiAutoresCLase.Models;

namespace WebApiAutoresCLase.Controllers
{
    // Endpoint es algo que podemos acceder dependiendo de la politicaa (En este caso son los metodos)

    // Creacion del controlador
    [ApiController]//Se especifica
    // Lo tome automaticamente deacuerdo al nombre de la clase va entre []
    // el problema que si es publica al cambiar la clase el fronted puede dar eror
    [Route("api/[controller]")]//Ruta y la direcciion que va atener

    //No poder acceder a los endpoints si no estamos logeados (Tambien se puede hacer a nivel de endpoint, metodos)
    // de manera global no es mejor tenerlo porque no podiramos hacer nada como para registrarse
    // Aqui es para agregar un nivel de seguridad Global (Policy)

    public class CuentasController : Controller
    {
        //para usar el identiti con usarmanager (Poder gestionar los usuarios)
        private readonly UserManager<ApplicationUser> _userManager;
        // para hacer uso de la llave
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        // Para usar el Mappeo (Configurar en el Program que vamos a usar AutoMapper)
        private readonly IMapper _mapper;
        public CuentasController(UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager; // Con este podmeos usar la autenticacion (Login)
            _mapper = mapper;
        }

        // Endpoint para registrar usuarios
        [HttpPost("registrar")]//Qie tipo de verbo html debe ir POST
        //resibe respuesta de autenitcacion que es el metodo
        // El action puede regresar culaquie cosa y retorna el token RespuestaAutenticacion y como parametros se loe pasa lo que tenemos que mandarle a este token
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CreacionUsuarioDTO creacionUsuarioDTO)//Vemos a pasarle un correo y password
        {
            // Variables de usuario de tipo identiti (GEneramos el usario) en este caso nos sirve que los dos sean email
            var usuario = new ApplicationUser { UserName = creacionUsuarioDTO.Email, Email = creacionUsuarioDTO.Email };
            // manager para manejar las cuentas de los usarios (Aqui creamos el usairio en la base de datos)
            // userManager en el contructor se tiene que inyectar

            usuario.Nombre = creacionUsuarioDTO.Nombre;
            usuario.ApellidoPaterno = creacionUsuarioDTO.ApellidoPaterno;
            usuario.ApellidoMaterno = creacionUsuarioDTO.ApellidoMaterno;
            usuario.Colonia = creacionUsuarioDTO.Colonia;
            usuario.Calle = creacionUsuarioDTO.Calle;
            usuario.numeroExterior = creacionUsuarioDTO.numeroExterior;
            usuario.numeroInteriro = creacionUsuarioDTO.numeroInteriro;
            usuario.Email = creacionUsuarioDTO.Email;
            usuario.PasswordHash = creacionUsuarioDTO.Password;
            usuario = _mapper.Map<ApplicationUser>(creacionUsuarioDTO);
            usuario.UserName = creacionUsuarioDTO.Email;
            
            var resultado = await _userManager.CreateAsync(usuario, creacionUsuarioDTO.Password);
            // en el resultado guardamos lo que nos regresa la base de datos
            if (resultado.Succeeded)
            {
                //SE contruye token para que so nos identifique esta logeado en la BD y poder entrar
                // para que se pueda logear
                return await ConstruirToken(creacionUsuarioDTO);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        // Metodo para contruis token Este no es parte del endpoint por eso no lleva verbos
        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario) {
            // Con claims vemos informacion de los datos que queremos saber en cada JsonWebTokne
            var claims = new List<Claim>()
            {
                new Claim("email",credencialesUsuario.Email),
            };

            // Agregar el claim ESto es para roles (Para tenerlos en el Token)
            //claims.AddRange();
            var usuario = await _userManager.FindByEmailAsync(credencialesUsuario.Email); // obtener usuiaro
            // Obtener claims de la BD
            var claimsBD = await _userManager.GetClaimsAsync(usuario); // sus roles
            // Anexamos los claims
            claims.AddRange(claimsBD);
            // Con esto antes solo teniamos el correo
            // ahora vienen con el rol en el token y podemos usar los metodos que pidan un rol para entrar


            //CReacion de llave y la decodificamos
            // Esta configuracion es para tener elementos que no sean variables de entorno si no aqui en appsettings
            // Ahi creamos una llave ahi la usa el JWT para hacer la encriptacion y ponemos los caracteres que queramos
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["LlaveJWT"]));

            // Generear las credenciales (LLAve y algoritmo)
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            // tiempo que va a durar el token (DAtys,Hours, months)
            var expiracion = DateTime.UtcNow.AddDays(1);// en este caso el token durara un dia

            // Generar el token (CREACION)
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds) ;

            return new RespuestaAutenticacion
            {
                // Esto es como lo tengamos con DTO RespuestaAutentucacion para pasar con mayusculas o minusculas
                token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                expiracion = expiracion,
            };

            // al ejecutar nos genera un token
            // si vamos a la base de datos: select * from "aspnetuser" anu
            // tenemos el usuario generado con todos los datos

            // el token lo poedemos copiar y lo pegamos a jwt.io
            // en Encoded y sale en Decoded
        }

        // nececitamos regresar el token (Respuesta autentificacion) para saber que esta registrado
        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario) 
        {
            // como la pass esta encriptada checamos con esto
            // con lockblock que no bloquee si se equivoca
            var resultado = await _signInManager.PasswordSignInAsync(credencialesUsuario.Email, credencialesUsuario.Password, isPersistent: false, lockoutOnFailure:false);

            // si Ok
            if (resultado.Succeeded)
            {
                //construimos el token
                return await ConstruirToken(credencialesUsuario);
            }
            else {
                // por cuestiones de seguridad
                // No deceimos en que se equivoco
                return BadRequest("Login incorrecto");
            }
        }

        // Renovar el token (Para que no pida la contrasena cuando expire la sesion y seguimos activos en la cuenta 
        // aqui ya tenemos el token generado en la base de datos
        [HttpGet("RenovarToken")]
        public async Task<ActionResult<RespuestaAutenticacion>> RenovarToken() {
            //recuperar el email de los claims del token (obtenemos que el usuario le buscamos el email y traemos el primer registro con ese email)
            var emailClaim = HttpContext.User.Claims.Where(x => x.Type == "email").FirstOrDefault();

            // si lo encuentra renovamos el token
            // al metodo contruir token recive un credencial usuario y le diremos que el email igual a que recueramos arriba
            var credencialesUsuario = new CredencialesUsuario() {
                Email = emailClaim.Value
            }; // esto lo tenemos en el JWT

            return await ConstruirToken(credencialesUsuario);

        }// Por defecto SWAGGER no puede hace reste tipo de autenticacion Vamos a configurarlo en Programa

        [HttpPost("HacerAdmin")]
        public async Task<ActionResult> HacerAdmin(AgregarRol agregarRol) 
        {
            //buscamos al usuario con su email
            var usuario = await _userManager.FindByEmailAsync(agregarRol.Email);
            if (usuario != null) {
                // nos pide usarios y claim (Le pasamos el que creamos el admin y si tiene 1 es que es admin)
                await _userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));
                return NoContent();
            }
            return BadRequest();
        }
        // Podria asignar un rol y eliminar el rol para todos no solo para admin
        // rol 
        // SE almacenan en la tabla AspNetUserClaims
        // Tennemos un usuario ya registrado con un correo al que podemos hacer admin

        [HttpPost("RemoverAdmin")]
        // este endpoint solo es accedido a usaurios logeados, no importa su claim
        // sale 401 (No autorizado)

        // login (Copia token sin comillas)
        // ahutorize
        // Bearer Token
        // abrimos este metodo y debe salir 204

        // Nivel controlador y Nivel Endpoint podemos tener diferentes politicas de seguridad
        // en program creamos politicas 'AddPoliciy' los que tengan ese claim 
        // Ahora solo abre cuando sea Admin
        // 403 (No tenemos permisos para entrar) (Si se le quito el admin)
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> RemoverAdmin(AgregarRol agregarRol)
        {
            //buscamos al usuario con su email
            var usuario = await _userManager.FindByEmailAsync(agregarRol.Email);
            if (usuario != null)
            {
                // nos pide usarios y claim (Le pasamos el que creamos el admin y si tiene 1 es que es admin)
                await _userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));
            }
            return BadRequest();
        }
    }
}
