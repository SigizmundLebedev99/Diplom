import Register from '../components/onApp/register'
import AfterRegistration from '../components/onApp/after-registration'
import TitlePage from '../components/onApp/title-page'
import Projects from '../components/onApp/projects'
import Begin from '../components/onApp/begin'
import Project from '../components/onProject/project'
import Home from '../components/onProject/home'
import Dashboard from '../components/onProject/dashboard'
import Epick from '../components/onProject/workItems/epick'
import UserStory from '../components/onProject/workItems/user-story'
import Task from '../components/onProject/workItems/task'
import Invites from '../components/onApp/invites'
import { ifAuthenticated } from './hooks'

export const routes = [
  {
    path:'/',
    component: Begin, 
    children:[
      {name:"title", path:'/',component:TitlePage},
      {name:"register", path:'/registration',component:Register},
      {name:"afterRegister", path:'/afterregistration', component:AfterRegistration},
      {name:"projects", path:'/projects', component:Projects, beforeEnter: ifAuthenticated},
      {name:"invites", path:'/invites',component:Invites, beforeEnter: ifAuthenticated}
    ]
  },
  {
    path:'/project/:projId', component: Project, beforeEnter: ifAuthenticated,
    children:[
      {name:'project', path:'/', component: Home, beforeEnter: ifAuthenticated},
      {name:'dashboard', path:'dashboard', component: Dashboard, beforeEnter: ifAuthenticated},
      {name:'EPICK', path:'epick-:number', component: Epick, beforeEnter:ifAuthenticated},
      {name:'STORY', path:'story-:number', component: UserStory, beforeEnter:ifAuthenticated},
      {
        name:'TASK', path:'task-:number', component: Task, beforeEnter:ifAuthenticated,
        meta:{color:'green', name:'Task'}
      },
      {
        name:'BUG', path:'bug-:number', component: Task, beforeEnter:ifAuthenticated,
        meta:{color:'red', name:'Bug'}
      },
      {
        name:'ISSUE', path:'issue-:number', component: Task, beforeEnter:ifAuthenticated,
        meta:{color:'orange', name:'Issue'}
      }
    ]
  }
]
