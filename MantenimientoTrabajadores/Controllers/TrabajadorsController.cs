using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MantenimientoTrabajadores.Data;
using MantenimientoTrabajadores.Models;

namespace MantenimientoTrabajadores.Controllers
{
    public class TrabajadorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrabajadorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trabajadors
        public async Task<IActionResult> Index(string sexo)
        {
            List<Trabajador> trabajadores;

            if (string.IsNullOrEmpty(sexo))
            {
                // Sin filtro sexo
                trabajadores = await _context.Trabajadores
                    .FromSqlRaw("EXEC sp_ListarTrabajadores")
                    .ToListAsync();
            }
            else
            {
                // Con filtro por sexo (SP)
                trabajadores = await _context.Trabajadores
                    .FromSqlRaw("EXEC sp_ListarTrabajadoresPorSexo @Sexo={0}", sexo)
                    .ToListAsync();
            }

            return View(trabajadores);
        }

        // GET: Trabajadors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trabajador == null)
            {
                return NotFound();
            }

            return View(trabajador);
        }

        // GET: Trabajadors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trabajadors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trabajador trabajador)
        {
            if (ModelState.IsValid)
            {
                if (trabajador.FotoFile != null)
            {
                // Ruta: raíz/wwwroot/fotos
                string uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "fotos"
                );

                // Crea la carpeta si no existe
                Directory.CreateDirectory(uploadsFolder);

                // Nombre único del archivo
                string fileName = Guid.NewGuid().ToString() +
                                  Path.GetExtension(trabajador.FotoFile.FileName);

                // Ruta completa del archivo
                string filePath = Path.Combine(uploadsFolder, fileName);

                // Guardar archivo en disco
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await trabajador.FotoFile.CopyToAsync(stream);
                }

                // Guardar solo el nombre en la BD
                trabajador.Foto = fileName;
            }

            _context.Add(trabajador);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            
                
            }
            return View(trabajador);
        }

        // GET: Trabajadors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadores.FindAsync(id);
            if (trabajador == null)
            {
                return NotFound();
            }
            return View(trabajador);
        }

        // POST: Trabajadors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trabajador trabajador)
        {
            if (id != trabajador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //Si el usuario sube una nueva foto
                if (trabajador.FotoFile != null)
                {
                    string uploadsFolder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "fotos"
                    );

                    Directory.CreateDirectory(uploadsFolder);

                    //borrar foto anterior
                    if (!string.IsNullOrEmpty(trabajador.Foto))
                    {
                        string oldFilePath = Path.Combine(uploadsFolder, trabajador.Foto);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    //nueva foto
                    string newFileName = Guid.NewGuid().ToString() +
                                         Path.GetExtension(trabajador.FotoFile.FileName);

                    string newFilePath = Path.Combine(uploadsFolder, newFileName);

                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        await trabajador.FotoFile.CopyToAsync(stream);
                    }

                    trabajador.Foto = newFileName;
                }

                try
                {
                    _context.Update(trabajador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrabajadorExists(trabajador.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(trabajador);
        }

        // GET: Trabajadors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trabajador = await _context.Trabajadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trabajador == null)
            {
                return NotFound();
            }

            return View(trabajador);
        }

        // POST: Trabajadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trabajador = await _context.Trabajadores.FindAsync(id);
            if (trabajador != null)
            {
                _context.Trabajadores.Remove(trabajador);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrabajadorExists(int id)
        {
            return _context.Trabajadores.Any(e => e.Id == id);
        }
    }
}
