import { NgModule } from '@angular/core'
import { ProfileComponent } from './components/profile.component'
import { SharedModule } from '../shared/shared.module'
import { RouterModule } from '@angular/router'
import { profileRoutes } from './profile.routes'

@NgModule({
  declarations: [ProfileComponent],
  imports: [SharedModule, RouterModule.forChild(profileRoutes)],
  exports: [],
  providers: [],
})
export class ProfileModule {}
