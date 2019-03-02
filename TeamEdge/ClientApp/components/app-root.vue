<template>
  <v-app>
        <v-toolbar color="primary" dark>
          <v-toolbar-side-icon></v-toolbar-side-icon>

          <v-toolbar-title class="title">TEAM EDGE</v-toolbar-title>
          <v-layout row align-center>
            <v-spacer/>
            <v-btn v-if="!isLoggedIn" @click="openSignInDialog()" flat>Войти</v-btn>
            <template v-else>
              <v-subheader>Lebedev Dmitriy</v-subheader>
              <v-avatar size="36px" class="mr-3">
                <img v-if="profile.avatar"
                  :src="profile.avatar"
                  alt="Avatar">
                <v-icon size="36px" dark v-else>account_circle</v-icon>
              </v-avatar>
            </template>
            <v-menu offset-y>
                <template v-slot:activator="{ on }">
                  <v-btn v-show="isLoggedIn" icon v-on="on" round flat>
                    <v-icon>more_vert</v-icon>
                  </v-btn>
                </template>
                <v-list>
                  <v-list-tile @click="signOut()">    
                    <v-list-tile-avatar>
                      <v-icon>
                        face
                      </v-icon>
                    </v-list-tile-avatar>            
                    <v-list-tile-title>Профиль</v-list-tile-title>
                  </v-list-tile>
                  <v-list-tile @click="signOut()">  
                    <v-list-tile-avatar>
                      <v-icon>
                        cancel
                      </v-icon>
                    </v-list-tile-avatar>               
                    <v-list-tile-title>Выйти</v-list-tile-title>
                  </v-list-tile>
                </v-list>
              </v-menu>
          </v-layout>
        </v-toolbar>

    <login></login>
  </v-app>
</template>

<script>
    import Login from './login'
    import { mapMutations, mapGetters} from 'vuex';


    export default {
      mounted(){
        this.signedIn();
      },
      components: {
        'login': Login
      },
      data () {
        return {
          drawer:false
        }
      },
      methods:{
        ...mapMutations({
          signedIn:'auth/signedIn',
          setOpened:'auth/setOpened',
          signOut:'auth/signOut'
        }),
        openSignInDialog(){
          this.setOpened(true);
        }
      },
      computed:{
        ...mapGetters({
          isLoggedIn:'auth/isLoggedIn',
          profile:'auth/profile'
        })
      }
    }
</script>

<style>
.title{
  font-family: "League Spartan", sans-serif;
  font-weight: bold;
}
</style>
