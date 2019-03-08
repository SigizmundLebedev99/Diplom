import Register from '../components/register'
import AfterRegistration from '../components/after-registration'
import TitlePage from '../components/title-page'
export const routes = [
  {name:"title", path:'/',component:TitlePage},
  {name:"register", path:'/registration',component:Register},
  {name:"afterRegister", path:'/afterregistration', component:AfterRegistration}
]
