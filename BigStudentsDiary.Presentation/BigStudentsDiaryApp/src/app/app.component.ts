import {Component, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {AuthModalComponent} from "./auth-modal/auth-modal.component";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
// export class AppComponent {
//   title = 'BigStudentsDiaryApp';
// }
export class AppComponent implements OnInit {
  constructor(private dialog: MatDialog) { }

  ngOnInit(): void {
    this.openAuthModal();
  }

  openAuthModal(): void {
    this.dialog.open(AuthModalComponent, {
      width: '400px',
      disableClose: true // Запрещает закрытие модального окна кликом вне его области
    });
  }
}
