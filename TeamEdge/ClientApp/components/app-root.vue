<template>
  <v-app>
        <v-toolbar color="primary" dark>
          <v-toolbar-side-icon></v-toolbar-side-icon>

          <v-toolbar-title class="title">TEAM EDGE</v-toolbar-title>

          <v-spacer></v-spacer>
          <v-btn @click="openSignInDialog()">sign in</v-btn>
        </v-toolbar>

    <login></login>
  </v-app>
</template>

<script>
    import Login from './login'
import { mapMutations } from 'vuex';

    export default {
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
          setOpened:'auth/setOpened'
        }),
        openSignInDialog(){
          this.setOpened(true);
        }
      }
    }
</script>

<style>
.title{
  font-family: "League Spartan", sans-serif;
  font-weight: bold;
}
</style>
