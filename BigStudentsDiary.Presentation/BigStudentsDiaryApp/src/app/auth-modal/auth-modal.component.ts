import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { GroupService } from '../services/group.service';

@Component({
  selector: 'app-auth-modal',
  templateUrl: './auth-modal.component.html',

})
export class AuthModalComponent implements OnInit {
  loginForm: FormGroup;
  registerForm: FormGroup;
  groups: any[] = [];

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AuthModalComponent>,
    private groupService: GroupService
  ) {
    this.loginForm = this.fb.group({
      login: ['', Validators.required],
      password: ['', Validators.required]
    });

    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      login: ['', Validators.required],
      password: ['', Validators.required],
      groupId: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.groupService.getAllGroups().subscribe(groups => {
      this.groups = groups;
    });
    this.openAuthModal();
  }

  onTabChange(event: any) {
    // Logic to handle tab change if needed
  }

  onLogin() {
    if (this.loginForm.valid) {
      // Handle login
      console.log(this.loginForm.value);
      this.dialogRef.close();
    }
  }

  onRegister() {
    if (this.registerForm.valid) {
      // Handle registration
      console.log(this.registerForm.value);
      this.dialogRef.close();
    }
  }

  openAuthModal() {
    const modalRef = this.modalService.open(AuthModalContentComponent, { centered: true }); // Открываем модальное окно
  }
}
