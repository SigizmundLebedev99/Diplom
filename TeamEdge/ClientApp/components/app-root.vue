<template>
  <v-app>
    <v-toolbar color="primary" dark>
      <v-toolbar-title class="font-weight-bold" @click="$router.push('/')">
        <img src="/logos/TEcut.png" class="mr-2" :height="ofSize({xs:50,sm:46,md:55})"/>
        <span v-show="ofSize({xs:false, sm:true})">TEAM EDGE</span>
      </v-toolbar-title>
      <v-layout row align-center justify-end>
        <template v-if="!profile">
          <v-btn @click="openSignInDialog()" flat>Войти</v-btn>
        </template>
        <template v-else>
          <span class="white--text">{{profile.fullName}}</span>
          <v-avatar size="36px" class="ml-3">
            <img v-if="profile.avatar"
            :src="profile.avatar"
            alt="Avatar">
            <v-icon size="36px" dark v-else>account_circle</v-icon>
          </v-avatar>
        </template>
        <side-menu/>
      </v-layout>
    </v-toolbar>
    <router-view class="view"></router-view>
    <login></login>
    <v-footer height="auto" class="footer-keeper">
      <v-layout column class="grey darken-2 white--text text-xs-center">
        <v-container>
          <p>Контактные данные:</p>
          <v-layout row align-center justify-center wrap>
            <v-flex xs12 md4 class="text-no-wrap">Email: dilebedev99@gmail.com</v-flex>
            <v-flex xs12 md4 class="text-no-wrap">Телефон: 8-(917)-264-38-50</v-flex>
          </v-layout>
        </v-container>
        <v-divider></v-divider>
        <p>&copy;2018 — Team Edge</p>
      </v-layout>
    </v-footer>
  </v-app>
</template>

<script>
    import Login from './login'
    import onResize from "../mixins/on-resize"
    import SideMenu from './side-menu'
    import { mapMutations, mapGetters, mapActions} from 'vuex';
    export default {
      mixins:[onResize],
      mounted(){
        this.signedIn();
      },
      components: {
        'login': Login,
        'side-menu':SideMenu
      },
      data () {
        return {
          drawer:false
        }
      },
      methods:{
        ...mapActions({
          signedIn:'auth/signedIn'
        }),
        ...mapMutations({
          setOpened:'auth/setOpened',
          signOut:'auth/signOut',
        }),
        openSignInDialog(){
          this.setOpened(true);
        }
      },
      computed:{
        ...mapGetters({
          profile:'auth/profile'
        })
      }
    }
</script>

<style scoped>
.view{
  min-height: 100%;
}
</style>
