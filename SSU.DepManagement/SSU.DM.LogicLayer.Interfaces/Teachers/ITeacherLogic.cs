﻿using Models.View;

namespace SSU.DM.LogicLayer.Interfaces.Teachers;

public interface ITeacherLogic
{
    IReadOnlyList<TeacherViewItem> GetAll();

    IReadOnlyList<TeacherCapacitiesViewItem> GetTeacherCapacities();

    void Create(TeacherViewItem viewItem);

    void Update(TeacherViewItem viewItem);

    void Delete(long id);
}