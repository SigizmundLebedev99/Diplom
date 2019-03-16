<template>
  <v-dialog :width="ofSize({xs:'300px', sm:'450px' , md:'500px'})" v-model="dialog">
    <v-card class="elevation-12">
      <v-toolbar dark color="primary">
        <v-toolbar-title>Вход</v-toolbar-title>
      </v-toolbar>
      <v-card-text>
        <v-form ref="form" v-model="valid">
          <v-text-field 
          v-model="model.login" 
          prepend-icon="person" 
          label="Логин"
          :placeholder="loginP"
          :rules="logEmptRule"
          required
          type="text"></v-text-field>
          <v-text-field
          :placeholder="passP"
          v-model="model.password"
          prepend-icon="lock" 
          label="Пароль"
          :rules="passEmptRule"
          required
          type="password"></v-text-field>
        </v-form>
        
      </v-card-text>
      <v-card-actions>
        <v-layout row justify-space-between>
        <v-btn class="ml-2" color="primary" @click="register()" flat small>Регистрация</v-btn>
        <v-btn class="mr-2" color="primary" @click="forgotPass()" flat small>Забыли пароль?</v-btn>
        </v-layout>
      </v-card-actions>
      <v-card-actions>
          <v-checkbox color="primary"
              class="ml-2"
              v-model="remember"
              label="Запомнить меня"></v-checkbox>
          <v-btn color="primary" class="mr-2" @click="onSignIn" :loading="loading">Войти</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script>
  import { mapMutations, mapGetters, mapActions} from "vuex";
  import onResize from "../mixins/on-resize"
  import formValidation from "../mixins/form-validation"
  export default {
    mixins:[onResize, formValidation],
    data(){
      return{
        loading:false,
        valid:false,
        model:{
          login:"",
          password:""
        },
        loginP:"",
        passP:"",
        remember:true,
        logEmptRule:[v=>!!v||"Необходимо ввести логин",
         v=>!/^.*\s.*$/.test(v) || "Логин не должен содержать пробелов"],
        passEmptRule:[v=>!!v||"Необходимо ввести пароль",
          v=>!/^.*\s.*$/.test(v) || "Пароль не должен содержать пробелов"]
      }
    },
    methods:{
      onSignIn(){
        this.clean();
        this.validate();
        if(this.valid){
          this.loading = true;
          this.$http.post('/api/account/token', this.model)
          .then(
            r=>
            {
              this.signIn({remember:this.remember, profile:r.data});
              this.loading = false;
            },
            r=>{
              var alias = r.response.data.Alias
              if(alias === "user_nf"){
                this.reset();
                this.loginP = "Неверный логин";
              }
              else if(alias === "password_inv"){
                this.model.password = "";
                this.passP = "Неверный пароль";
              }
              else if(alias === "email_not_confirmed"){
                this.reset();
                this.passP = "Email не подтвержден";
              }
              this.loading = false;
            }
          )
        }
      },
      ...mapMutations({
        setOp:'auth/setOpened'
      }),
      ...mapActions({
        signIn:'auth/signIn'
      }),
      register(){
        this.dialog = false;
        this.$router.push("/registration");
      },
      forgotPass(){
        this.dialog = false;
        this.$router.push("/forgotpass");
      },
      clean(){
        this.loginP="";
        this.passP="";
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
          this.clean();
        }
      }
    }
  }
</script>

