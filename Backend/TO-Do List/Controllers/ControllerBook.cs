using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using TO_Do_List.Model;

namespace TO_Do_List.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControllerBook: ControllerBase
    {
        private readonly DBContext _context;

        public ControllerBook(DBContext context)
        {
            _context = context;
        }

        /////////Get task data
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            try
            {
                var books = await _context.Book
                    .FromSqlRaw("EXEC spGetBooks")
                    .ToListAsync();

                return Ok(books);
            }
            catch (Exception ex)
            {
                // Opcional: log en consola o logger
                Console.WriteLine($"Error en GetBooks: {ex.Message}");

                return StatusCode(500, new
                {
                    message = "Ocurrió un error al obtener los datos.",
                    error = ex.Message
                });
            }
        }
        ////
        ///
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var pIdB = new SqlParameter("@IdB", id);

                var book = await _context.Book
                    .FromSqlRaw("EXEC spGetBooks @IdB", pIdB)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (book == null)
                    return NotFound(new { message = "No se encontró el book con ese Id." });

                return Ok(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetBook: {ex.Message}");
                return StatusCode(500, new
                {
                    message = "Ocurrió un error al obtener el book.",
                    error = ex.Message
                });
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>

        ////////
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            if (book == null)
                return BadRequest("El objeto Book es requerido.");

            try
            {
                var pIdUser = new SqlParameter("@IdUserFK", SqlDbType.Int)
                {
                    Value = book.IdUserFK
                };
                var pTitle = new SqlParameter("@TitleB", SqlDbType.VarChar, 150)
                {
                    Value = book.TitleB
                };
                var pDesc = new SqlParameter("@DescriptionB", SqlDbType.VarChar)
                {
                    Value = (object?)book.DescriptionB ?? DBNull.Value
                };
                var pStatus = new SqlParameter("@StatusB", SqlDbType.VarChar, 40)
                {
                    Value = (object?)book.StatusB ?? DBNull.Value
                };

                // Ejecuta el SP (inserta en la BD)
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC spCreateBook @IdUserFK, @TitleB, @DescriptionB, @StatusB",
                    pIdUser, pTitle, pDesc, pStatus
                );

                return Ok(new { message = "Book creado correctamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CreateBook: {ex.Message}");
                return StatusCode(500, new
                {
                    message = "Ocurrió un error al crear el book.",
                    error = ex.Message
                });
            }
        }

        //
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
        {
            if (book == null || id != book.IdB)
                return BadRequest("El Id de la ruta no coincide con el del objeto.");

            try
            {
                var pIdB = new SqlParameter("@IdB", id);
                var pIdUser = new SqlParameter("@IdUserFK", book.IdUserFK);
                var pTitle = new SqlParameter("@TitleB", book.TitleB ?? (object)DBNull.Value);
                var pDesc = new SqlParameter("@DescriptionB", (object?)book.DescriptionB ?? DBNull.Value);
                var pStatus = new SqlParameter("@StatusB", (object?)book.StatusB ?? DBNull.Value);

                var rows = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC spUpdateBook @IdB, @IdUserFK, @TitleB, @DescriptionB, @StatusB",
                    pIdB, pIdUser, pTitle, pDesc, pStatus
                );

                if (rows == 0)
                    return NotFound(new { message = "No se encontró el book a actualizar." });

                return Ok(new { message = "Book actualizado correctamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateBook: {ex.Message}");
                return StatusCode(500, new
                {
                    message = "Ocurrió un error al actualizar el book.",
                    error = ex.Message
                });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var pIdB = new SqlParameter("@IdB", id);

                var rows = await _context.Database.ExecuteSqlRawAsync(
                    "EXEC spDeleteBook @IdB",
                    pIdB
                );

                if (rows == 0)
                    return NotFound(new { message = "No se encontró el book a eliminar." });

                return Ok(new { message = "Book eliminado correctamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en DeleteBook: {ex.Message}");
                return StatusCode(500, new
                {
                    message = "Ocurrió un error al eliminar el book.",
                    error = ex.Message
                });
            }
        }



    }
}
