using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TodoManager.Data;
using TodoManager.Models;
using System.Web.Http.Cors;
using System;

namespace TodoManager.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TodoesController : ApiController
    {
        private dbContext db = new dbContext();

        // GET: api/Todoes/getAllTodoes
        public IQueryable<Todo> getAllTodoes()
        {
            return db.Todoes;
        }

        // GET: api/Todoes/getTodo/{id}
        [ResponseType(typeof(Todo))]
        public async Task<IHttpActionResult> getTodo(string id)
        {
            Todo Todo = await db.Todoes.FindAsync(id);
            if (Todo == null)
            {
                return NotFound();
            }

            return Ok(Todo);
        }

        // GET: api/Todoes/getRandomTodo
        [ResponseType(typeof(Todo))]
        public async Task<IHttpActionResult> getRandomTodo()
        {
            var randomTodo = await db.Todoes.OrderBy(r => Guid.NewGuid()).Take(1).FirstOrDefaultAsync();

            if (randomTodo == null)
            {
                return BadRequest();
            }
            randomTodo.value = ReverseString(randomTodo.value);
            return Ok(randomTodo);
        }

        // PUT: api/Todoes/updateTodo/{id}
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> updateTodo(string id, Todo Todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Todo.id)
            {
                return BadRequest();
            }

            db.Entry(Todo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Todoes/createTodo
        [ResponseType(typeof(Todo))]
        public async Task<IHttpActionResult> createTodo(Todo Todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Todoes.Add(Todo);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = Todo.id }, Todo);
        }

        // DELETE: api/Todoes/deleteTodo/{id}
        [ResponseType(typeof(Todo))]
        public async Task<IHttpActionResult> deleteTodo(string id)
        {
            Todo Todo = await db.Todoes.FindAsync(id);
            if (Todo == null)
            {
                return NotFound();
            }

            db.Todoes.Remove(Todo);
            await db.SaveChangesAsync();

            return Ok(Todo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TodoExists(string id)
        {
            return db.Todoes.Count(e => e.id == id) > 0;
        }

        private string ReverseString(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length < 2)
                return str;

            char[] strChars = str.ToCharArray();
            int start = -1, end = strChars.Length;
            char temp;

            while (++start < --end)
            {
                temp = strChars[start];
                strChars[start] = str[end];
                strChars[end] = temp;
            }

            return string.Join("", strChars);
        }
    }
}