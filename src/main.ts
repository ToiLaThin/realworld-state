import { bootstrapApplication } from '@angular/platform-browser'
import { AppComponent } from './app/app.component'
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic'
import { AppModule } from './app/app.module'

const bootstrapAppPromise = platformBrowserDynamic().bootstrapModule(AppModule)
bootstrapAppPromise
  .then(success => console.log(`Bootstrap success`))
  .catch(err => console.error(err))
