import Register from '../components/on-app/register'
import AfterRegistration from '../components/on-app/after-registration'
import TitlePage from '../components/on-app/title-page'
import Projects from '../components/on-app/projects'
import Begin from '../components/on-app/begin'
import Project from '../components/on-project/project'
import dashboard from '../components/on-project/dashboard'
import Backlog from '../components/on-project/backlog'
import Epick from '../components/on-project/workItems/epick'
import UserStory from '../components/on-project/workItems/user-story'
import Task from '../components/on-project/workItems/task'
import Invites from '../components/on-app/invites'
import Profile from '../components/on-app/profile'
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
      {name:"invites", path:'/invites',component:Invites, beforeEnter: ifAuthenticated},
      {name:"profile", path:'/profile',component:Profile, beforeEnter: ifAuthenticated}
    ]
  },
  {
    path:'/project/:projId', component: Project, beforeEnter: ifAuthenticated,
    children:[
      {name:'project', path:'/', component: dashboard, beforeEnter: ifAuthenticated},
      {name:'backlog', path:'backlog', component: Backlog, beforeEnter: ifAuthenticated},
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
