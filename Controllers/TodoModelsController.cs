using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Controller]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoModel
                {
                    Id = 1,
                    Name = "ali",
                    IsComplete = false
                }
                );

                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return new JsonResult(_context.TodoItems.ToList());
        }

        //[HttpGet]
        //public string Test()
        //{
        //    var a = new List<TodoModel>();
        //    return "test";
        //}

        //[HttpGet]
        //public ActionResult GetFirst()
        //{
        //    return new JsonResult(_context.TodoItems.First().Id);
        //}

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = new JsonResult(_context.TodoItems.Find(id));
            if (item == null)
            {
                return new ContentResult();
            }
            return item;
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoModel item)
        {

            _context.TodoItems.Add(item);
            
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id , [FromBody] TodoModel item)
        {
            var todo = _context.TodoItems.Find(id);
            if(todo == null)
            {
                return NotFound();
            }

            todo.Name = item.Name;
            todo.IsComplete = item.IsComplete;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();

            return NoContent();
        }


    }
}