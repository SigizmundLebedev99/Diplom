import Register from '../components/onApp/register'
import AfterRegistration from '../components/onApp/after-registration'
import TitlePage from '../components/onApp/title-page'
import Projects from '../components/onApp/projects'
import Begin from '../components/onApp/begin'
import Project from '../components/onProject/project'
import Home from '../components/onProject/home'
import Dashboard from '../components/onProject/dashboard'
import { ifAuthenticated } from './hooks'

export const routes = [
  {
    path:'/', 
    component: Begin, 
    children:[
      {name:"title", path:'/',component:TitlePage},
      {name:"register", path:'/registration',component:Register},
      {name:"afterRegister", path:'/afterregistration', component:AfterRegistration},
      {name:"projects", path:'/projects', component:Projects, beforeEnter: ifAuthenticated}
    ]
  },
  {
    path:'/project/:projId', component: Project, beforeEnter: ifAuthenticated,
    children:[
      {name:'project', path:'/', component: Home, beforeEnter: ifAuthenticated},
      {name:'dashboard', path:'dashboard', component: Dashboard, beforeEnter: ifAuthenticated}
    ]
  }
]
