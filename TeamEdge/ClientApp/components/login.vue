<template>
  <v-dialog :width="ofSize({xs:'300px', sm:'450px' , md:'500px'})" v-model="dialog">
    <v-card class="elevation-12">
      <v-toolbar dark color="primary">
        <v-toolbar-title>Войти</v-toolbar-title>
      </v-toolbar>
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-text-field v-model="logData.login" 
          prepend-icon="person" 
          label="Логин" 
          :rules="logEmptRule"
          required
          type="text"></v-text-field>
          <v-text-field 
          v-model="logData.password" 
          prepend-icon="lock" 
          label="Пароль"
          :rules="passEmptRule"
          required
          type="password"></v-text-field>
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-layout row justify-space-between align-center>
          <v-checkbox color="primary"
              class="ml-2"
              v-model="remember"
              label="Запомнить меня"></v-checkbox>
          <v-btn color="primary" class="mr-2" @click="onSignIn">Login</v-btn>
        </v-layout>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script>
  import { mapMutations, mapGetters} from "vuex";
  export default {
    data(){
      return{
        valid:false,
        logData:{
          login:"",
          password:""
        },
        remember:true,
        logEmptRule:[v=>!!v||"Необходимо ввести логин",
         v=>!/^.*\s.*$/.test(v) || "Логин не должен содержать пробелов"],
        passEmptRule:[v=>!!v||"Необходимо ввести пароль",
         v=>!/^.*\s.*$/.test(v) || "Пароль не должен содержать пробелов"]
      }
    },
    methods:{
      onSignIn(){
        this.validate();
        if(this.valid){
          this.reset();
        }
      },
      ...mapMutations({
        signIn:'auth/signIn',
        setOp:'auth/setOpened'
      }),
      validate () {
        if (this.$refs.form.validate()) {
          this.snackbar = true
        }
      },
      reset () {
        this.$refs.form.reset()
      },
      resetValidation () {
        this.$refs.form.resetValidation()
      }
    },
    computed:{
      dialog:{
        ...mapGetters({
          get:'auth/getOpened'
        }),
        set(value){
          this.setOp(value);
          this.reset();
          this.resetValidation();
        }
      }
    }
  }
</script>

