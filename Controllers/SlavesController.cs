using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _7semester_ASP_SecondTask.Data;
using _7semester_ASP_SecondTask.Models;

namespace _7semester_ASP_SecondTask.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SlavesController : ControllerBase
	{
		private readonly SlavesContext _context;

		public SlavesController(SlavesContext context)
		{
			_context = context;
		}

		// GET: api/Slaves
		[HttpGet]
		public async Task<ActionResult<IEnumerable<SlaveDTO>>> GetSlaves()
		{
			var slavesDTO = _context.Slaves
				.Include(x => x.Master)
				.Select(x => ToSlaveDTOMap(x));
			return await slavesDTO.ToListAsync();
		}

		// GET: api/Slaves/5
		[HttpGet("{id}")]
		public async Task<ActionResult<SlaveDTO>> GetSlave(int id)
		{
			var slave = await _context.Slaves
				.Include("Master")
				.FirstOrDefaultAsync(x => x.Id == id);
			//.FindAsync(id);

			if (slave == null)
			{
				return NotFound();
			}

			return ToSlaveDTOMap(slave);
		}

		// PUT: api/Slaves/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutSlave(int id, SlaveDTO slave)
		{
			var master = _context.Masters.Where(x => x.Name == slave.MasterName).FirstOrDefault();
			if (master != null)
			{
				_context.Entry(new Slave
				{
					Id = id,
					Price = slave.Price,
					SkinTone = slave.SkinTone,
					Age = slave.Age,
					Master = master,
					MasterId = master.Id
				}).State = EntityState.Modified;
			}
			else
			{
				_context.Entry(new Slave
				{
					Id = id,
					Price = slave.Price,
					SkinTone = slave.SkinTone,
					Age = slave.Age,
				}).State = EntityState.Modified;
			}
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SlaveExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Slaves
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Slave>> PostSlave(SlaveDTO slave)
		{
			var master = _context.Masters.Where(x => x.Name == slave.MasterName).FirstOrDefault();
			if (master != null)
			{
				_context.Slaves.Add(new Slave
				{
					Id = default,
					Price = slave.Price,
					SkinTone = slave.SkinTone,
					Age = slave.Age,
					Master = master,
					MasterId = master.Id
				});
			}
			else
			{
				_context.Slaves.Add(new Slave
				{
					Id = default,
					Price = slave.Price,
					SkinTone = slave.SkinTone,
					Age = slave.Age,
				});
			}
			await _context.SaveChangesAsync();

			return Ok();
		}

		// DELETE: api/Slaves/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSlave(int id)
		{
			var slave = await _context.Slaves.FindAsync(id);
			if (slave == null)
			{
				return NotFound();
			}

			_context.Slaves.Remove(slave);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool SlaveExists(int id)
		{
			return _context.Slaves.Any(e => e.Id == id);
		}

		private static SlaveDTO ToSlaveDTOMap(Slave slave)
		{
			//var name = _context.Masters.Where(x => x.Id == slave.MasterId).FirstOrDefault().Name;
			if (slave.MasterId != null)
				return new SlaveDTO()
				{
					Price = slave.Price,
					SkinTone = slave.SkinTone,
					Age = slave.Age,
					MasterName = slave.Master.Name
				};
			else
				return new SlaveDTO()
				{
					Price = slave.Price,
					SkinTone = slave.SkinTone,
					Age = slave.Age,
					MasterName = null
				};
		}
	}
}
