﻿using FlatManagement.Models;
using FlatManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nancy.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class FlatConfigController : Controller
    {
        private readonly FlatDBContext _context;
        
        public FlatConfigController(FlatDBContext context)
        {
            _context = context;
        }

        // GET: Flat
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            return View(await _context.FlatConfigs.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
        }



        // GET: Flat/Create
        [Authorize]
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new FlatConfigVM());
            else
                return View(_context.FlatConfigs.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(FlatConfigVM flatVM)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            FlatConfigVM addModel = null;// new FlatConfigVM();
            string[] alphaSerice = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string[] numericSerice = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26" };


            int Sequence = Convert.ToInt32(flatVM.Sequence.ToString());
            int FlatStartFrom = Convert.ToInt32(flatVM.FlatStartFrom.ToString());
            int TotalFlat = flatVM.TotalFlat == null ? 0 : Convert.ToInt32(flatVM.TotalFlat.ToString());
            int FlatPerFloor = flatVM.FlatPerFloor == null ? 0 : Convert.ToInt32(flatVM.FlatPerFloor.ToString());
            int flatStrNum = Convert.ToInt32(flatVM.FlatNo.ToString());

            int TotalFloor = flatVM.TotalWing == null ? 0 : Convert.ToInt32(flatVM.TotalWing.ToString());
            int WingStyle = flatVM.Wing == null ? 0 : Convert.ToInt32(flatVM.Wing.ToString());
            int FlatPerWing = flatVM.FlatPerWing == null ? 0 : Convert.ToInt32(flatVM.FlatPerWing.ToString());
            int WingPerFloor = flatVM.WingPerFloor == null ? 0 : Convert.ToInt32(flatVM.WingPerFloor.ToString());

            string DelimeterStr = flatVM.Delimeter == null ? null : flatVM.Delimeter.ToUpper();
            int GroundFloorStart = flatVM.GroundFloorStart == null ? 0 : Convert.ToInt32(flatVM.GroundFloorStart.ToString());

            //int countFloor = GroundFloorStart + FlatStartFrom + TotalFlat + TotalFloor;
            int countFloor = TotalFlat + TotalFloor;

            string flatNumber = "";
            int countID = 0;
            int countFloorLoop = FlatStartFrom + GroundFloorStart;

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (TotalFloor > 0 && WingStyle > 0 && FlatPerWing > 0)
                {
                    for (int t = 0; t < countFloor; t++)
                    {
                        
                        for (int wf = 0; wf < WingPerFloor; wf++)
                        {
                            string wingStyle = (WingStyle == 1) ? alphaSerice[wf].ToString() : numericSerice[wf].ToString();

                            for (int flat = 0; flat < FlatPerWing; flat++)
                            {
                                addModel = new FlatConfigVM();
                                countID++;
                                string flatStyle = (flatStrNum == 1) ? alphaSerice[flat].ToString() : numericSerice[flat].ToString();

                                switch (Sequence)
                                {
                                    case 1:
                                        flatNumber = wingStyle + DelimeterStr + countFloorLoop + DelimeterStr + flatStyle;
                                        break;
                                    case 2:
                                        flatNumber = flatStyle + DelimeterStr + countFloorLoop + DelimeterStr + wingStyle;
                                        break;
                                    case 3:
                                        flatNumber = flatStyle + DelimeterStr + wingStyle + DelimeterStr + countFloorLoop;
                                        break;
                                    default:
                                        flatNumber = wingStyle + DelimeterStr + countFloorLoop + DelimeterStr + flatStyle;
                                        break;
                                }
                                addModel.ApartCodeName = APART_CODE_LOCAL_VAR;
                                addModel.Floor = t;
                                addModel.WingName = wingStyle;
                                //addModel.Wing = wf;
                                // addModel.Id = countID;
                                addModel.FlatNo =  flatNumber;
                                addModel.TotalFlat = TotalFlat;
                                addModel.slId = countID; 
                                _context.FlatConfigs.Add(addModel);
                                _context.SaveChanges();

                            }
                        }
                        countFloorLoop++;
                    }
                }
                else
                {
                    for (int i = 0; i < countFloor; i++)
                    {
                        for (int f = 0; f < FlatPerFloor; f++)
                        {
                            addModel = new FlatConfigVM();
                            countID++;
                            string flatStyle = (flatStrNum == 1) ? alphaSerice[f].ToString() : numericSerice[f].ToString();

                            switch (Sequence)
                            {
                                case 1:
                                    flatNumber = countFloorLoop + DelimeterStr + flatStyle;
                                    break;
                                case 2:
                                    flatNumber = flatStyle + DelimeterStr + countFloorLoop;
                                    break;
                                default:
                                    flatNumber = flatStyle + DelimeterStr + countFloorLoop;
                                    break;
                            }
                            addModel.ApartCodeName = APART_CODE_LOCAL_VAR;
                            addModel.Floor = i;
                            addModel.WingName = "-";
                            //addModel.Id = countID;
                            addModel.FlatNo = flatNumber;
                            addModel.TotalFlat = TotalFlat;
                            addModel.slId = countID;
                            _context.FlatConfigs.Add(addModel);
                            _context.SaveChanges();
                        }
                        countFloorLoop++;
                    }


                }

            }
            return View(flatVM);
        }

        // GET: Employee/Delete/5
        //public async Task<IActionResult> Delete(int? SlId)
        //{
        //    if (SlId == null)
        //    {
        //        return NotFound();
        //    }
        //    var valObj = await _context.FlatConfigs.FindAsync(SlId);
        //    _context.FlatConfigs.Remove(valObj);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        // GET: Employee/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }


        //    var owners = await _context.FlatConfigs.FindAsync(id);
        //    _context.FlatConfigs.Remove(owners);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var findValue = await _context.FlatConfigs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (findValue == null)
            {
                return NotFound();
            }
            _context.FlatConfigs.Remove(findValue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //merge flat goes here
        // GET
        [Authorize]
        public IActionResult MergeFlats()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];

            bool IsWing = _context.FlatConfigs.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR)
                .Select(f => f.isWing).FirstOrDefault();

            //supposed to be IsWing but the DB has reverse value
            if (!IsWing)
            {
                //Select Wing
                var WingList = _context.FlatConfigs
                    .Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).Select(f => f.WingName).Distinct()
                    .Select(a => new SelectListItem
                    {
                        Text = a.ToString(),
                        Value = a.ToString()
                    });

                ViewData["Wing"] = WingList.ToList();
            };

            //select Flat
            var FlatList = from f in _context.FlatConfigs.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR && !_context.Users.Select(m => m.Flat_No)
                                     .Contains(e.FlatNo) || e.Merged == false)
                           select new SelectListItem()
                           {
                               Value = f.Id.ToString(),
                               Text = f.FlatNo
                           };

            ViewData["ToMerge"] = FlatList.ToList();
            ViewData["WithMerge"] = FlatList.ToList();


            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MergeFlats(MergeFlatsViewModel mergeFlatsViewModel)
        {
            var toMerge = Int32.Parse(mergeFlatsViewModel.ToMerge);
            var withMerge = Int32.Parse(mergeFlatsViewModel.WithMerge);
            var wing = mergeFlatsViewModel.Wing;

            var oldData = _context.FlatConfigs.Where(a => a.Id == toMerge).FirstOrDefault();

            var newData = new FlatConfigVM
            {
                ApartCodeName = oldData.ApartCodeName,
                WingName = oldData.WingName,
                Delimeter = oldData.Delimeter,
                FlatNo = mergeFlatsViewModel.NewFlatNo,
                TotalFlat = oldData.TotalFlat,
                Sequence = oldData.Sequence,
                //Wing = oldData.Wing,
                //FlatPerFloor = oldData.FlatPerFloor,
                //FlatStartFrom = oldData.FlatStartFrom,
                Merged = true,
                MergerCount = 2,
                Floor = oldData.Floor,
                slId = oldData.slId,
                //GroundFloorStart = oldData.GroundFloorStart

            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newData);
                    await _context.SaveChangesAsync();

                    var flats = await _context.FlatConfigs.FindAsync(toMerge);
                    _context.FlatConfigs.Remove(flats);
                    flats = await _context.FlatConfigs.FindAsync(withMerge);
                    _context.FlatConfigs.Remove(flats);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return View();
            }

            return View();
        }


    }
}
