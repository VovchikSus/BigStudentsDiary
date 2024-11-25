import {Component, OnInit} from '@angular/core';
import {NgForm} from "@angular/forms";
import {TimetableService} from "../shared/timetable.service";
import {Timetable} from "../shared/timetable.model";

@Component({
  selector: 'app-timetable-form',
  templateUrl: './timetable-form.component.html',
})
export class TimetableFormComponent implements OnInit {
  list: Timetable[] = [];
  constructor(public service: TimetableService) {
  }

  ngOnInit(): void {
    this.service.refreshList(-490224733);
  }

  OnSubmit(form: NgForm) {
    const formData = form.value;
    this.service.getTimetable(formData.groupId).subscribe({
        next: res => {
          this.service.list = res as Timetable[];
        },

        error: err => {
          console.log(err);
        }


      }
    )


  }
}
