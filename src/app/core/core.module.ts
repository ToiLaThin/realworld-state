import { NgModule } from '@angular/core'
import { HeaderComponent } from './layout/header.component'
import { FooterComponent } from './layout/footer.component'
import { RouterModule } from '@angular/router'
import { SharedModule } from '../shared/shared.module'
import { HTTP_INTERCEPTORS } from '@angular/common/http'
import { TokenInterceptor } from './interceptors/token.interceptor'

@NgModule({
  declarations: [HeaderComponent, FooterComponent],
  imports: [SharedModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },
  ],
  exports: [HeaderComponent, FooterComponent],
})
export class CoreModule {}
