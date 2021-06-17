﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hsm_api.Models;
using System.Net.Http;
using System.Text;
using hsm_api.Domain.StartProduction;

namespace hsm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        private readonly WebhookContext _context;
        private readonly StartProductionService _startProductionHandler;

        public WebhooksController(WebhookContext context, StartProductionService startProductionHandler)
        {
            _context = context;
            _startProductionHandler = startProductionHandler;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Webhook>> GetWebhook(long id)
        {
            var webhook = await _context.Webhooks.FindAsync(id);

            if (webhook == null)
            {
                return NotFound();
            }

            return webhook;
        }

        [HttpPost]
        public async Task<ActionResult<Webhook>> PostWebhook(Webhook webhook)
        {
            _context.Webhooks.Add(webhook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWebhook), new { id = webhook.Id }, webhook);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWebhook(long id)
        {
            var webhook = await _context.Webhooks.FindAsync(id);
            if (webhook == null)
            {
                return NotFound();
            }

            _context.Webhooks.Remove(webhook);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}