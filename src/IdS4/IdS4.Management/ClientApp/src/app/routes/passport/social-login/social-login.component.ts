import { Component, OnInit } from '@angular/core';
import { SocialService } from '@delon/auth';

@Component({
  selector: 'app-social-login',
  templateUrl: './social-login.component.html',
  styles: [],
  providers: [SocialService]
})
export class SocialLoginComponent implements OnInit {

  constructor(private socialService: SocialService) { }

  ngOnInit() {
    this.socialService.login('https://localhost:5001/connect/authorize', '/',
      {
        type: 'href'
      });
  }
}
