﻿using Models.View;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DbEntities;
using SSU.DM.LogicLayer.Interfaces.Teachers;

namespace SSU.DM.LogicLayer.Teachers;

public class TeacherLogic : ITeacherLogic
{
    private readonly ITeachersDao _teachersDao;
    private readonly ISemesterDao _semesterDao;


    public TeacherLogic(
        ITeachersDao teachersDao,
        ISemesterDao semesterDao)
    {
        _teachersDao = teachersDao;
        _semesterDao = semesterDao;
    }

    public IReadOnlyList<TeacherViewItem> GetAll()
    {
        return _teachersDao.GetAll().Select(x => x.ToViewItem()).ToList();
    }

    public IReadOnlyList<TeacherCapacitiesViewItem> GetTeacherCapacities()
    {
        var semesters = _semesterDao.GetAll();
        return _teachersDao.GetAll()
            .Select(teacher => CreateTeacherCapacitiesViewItem(teacher, semesters))
            .ToList();
    }

    private TeacherCapacitiesViewItem CreateTeacherCapacitiesViewItem(
        Teacher teacher,
        IReadOnlyList<Semester> semesters)
    {

        return new TeacherCapacitiesViewItem
        {
            Teacher = teacher.ToViewItem(),
            CapacityBySemester = semesters.ToDictionary(
                semester => semester.Id,
                semester =>
                {
                    return teacher.Requests
                            //.Where(request => request.SemesterId == semester.Id)
                            .Sum(request => request.TotalHours);
                })
        };
    }

    public void Create(TeacherViewItem viewItem)
    {
        _teachersDao.Add(new Teacher
        {
            FirstName = viewItem.FirstName,
            MiddleName = viewItem.MiddleName,
            LastName = viewItem.LastName,
            JobTitleId = viewItem.JobTitle.Id,
            Rate = viewItem.Rate,
        });
    }

    public void Update(TeacherViewItem viewItem)
    {
        _teachersDao.Update(new Teacher
        {
            Id = viewItem.Id,
            FirstName = viewItem.FirstName,
            MiddleName = viewItem.MiddleName,
            LastName = viewItem.LastName,
            JobTitleId = viewItem.JobTitle?.Id,
            Rate = viewItem.Rate,
        });
    }

    public void Delete(long id)
    {
        _teachersDao.DeleteById(id);
    }
}