using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newspaper.Data;
using Newspaper.Models;

namespace Newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly NewspaperdbContext _context;
        public User CurrentUser { get; set; } 

        public ArticlesController(NewspaperdbContext context)
        {
            _context = context;
        }
       

        // GET: api/Articles/Writer
        [HttpGet("Writer")]
        [Authorize(Roles ="Writer")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            CurrentUser = GetCurrentUser();
            List<Article> A = _context.Users.Find(CurrentUser.UsrID).Articles.ToList();

          if (A==null)
          {
              return NotFound();
          }
            else
            {
                return Ok(A);

            }

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticless()
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }
            return await _context.Articles.Where(o=>o.IsActive==true).IgnoreQueryFilters().ToListAsync();

        }
        [HttpPut("Toggle/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<Article>> ActivateDeactivate(int id)
        {
            Article A = _context.Articles.Find(id);
            if (A==null)
            {
                return BadRequest();
            }
            else
            {
                // A.IsActive == false ? A.IsActive = true : A.IsActive = false;
                A.IsActive = A.IsActive ? false : true;
                await _context.SaveChangesAsync();
                return Ok(A);
            }
        }



        // GET: api/Articles/5
        [HttpGet("{title}")]
        [AllowAnonymous]
        public async Task<ActionResult<Article>> GetArticle(string title)
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }
            var article = _context.Articles.Where(o=>o.Title==title).IgnoreQueryFilters().FirstOrDefault();

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            CurrentUser = GetCurrentUser();
            if (id != article.Art_ID)
            {
                return BadRequest();
            }

            //check if article is his article
            if(_context.Articles.Where(o=>o.Art_ID==id&& o.WriterID==CurrentUser.UsrID)!=null)
            {
                _context.Entry(article).State = EntityState.Modified;

            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(article);
        }

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            CurrentUser = GetCurrentUser();
            if (_context.Articles == null)
          {
              return Problem("Entity set 'NewspaperdbContext.Articles'  is null.");
          }
            article.WriterID = CurrentUser.UsrID;
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return Created("anything", article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            CurrentUser = GetCurrentUser();
            var article = await _context.Articles.FindAsync(id);
            if (_context.Articles == null)
            {
                return NotFound();
            }
            if (_context.Articles.Where(o => o.Art_ID == id && o.WriterID == CurrentUser.UsrID) == null)//doesnt exist or not his article
            {
                return NotFound();
            }
            else
            {
                _context.Articles.Remove(article);

            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return (_context.Articles?.Any(e => e.Art_ID == id)).GetValueOrDefault();
        }

        private  User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;// get identity of loggedin user

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return  new User
                {
                    UsrID = int.Parse(userClaims.FirstOrDefault(o=>o.Type=="ID")?.Value),
                    Username = userClaims.FirstOrDefault(o => o.Type == "Username")?.Value,
                    Password = userClaims.FirstOrDefault(o => o.Type == "Password")?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
          
        }
    }
}
