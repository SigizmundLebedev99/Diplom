import Register from '../components/register'
import AfterRegistration from '../components/after-registration'
import TitlePage from '../components/title-page'
import Projects from '../components/projects'
import { ifAuthenticated } from './hooks'

export const routes = [
  {name:"title", path:'/',component:TitlePage},
  {name:"register", path:'/registration',component:Register},
  {name:"afterRegister", path:'/afterregistration', component:AfterRegistration},
  {name:"projects", path:'/projects', component:Projects, beforeEnter: ifAuthenticated}
]
