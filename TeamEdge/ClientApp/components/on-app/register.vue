<template>
  <v-container>
    <v-layout>
      <v-flex xs10 offset-xs1 md4 offset-md4>
        <v-card>
          <v-card-title><span class="title">Регистрация</span></v-card-title>
          <v-card-text>
          <v-form ref="form" v-model="valid">
            <v-text-field
            v-model="model.firstName" 
            label="Имя" 
            :rules="defaultRule('login')"
            required></v-text-field>
            <v-text-field
            v-model="model.lastName" 
            label="Фамилия" 
            :rules="defaultRule('lname')"
            required></v-text-field>
            <v-text-field 
            v-model="model.patronymic" 
            label="Отчество"></v-text-field>
            <v-text-field 
            v-model="model.email" 
            label="Email"
            :rules="[...defaultRule('email'),emailRule, uniqueEmailRule]"></v-text-field>
            <v-text-field 
            v-model="model.userName"
            label="Логин" 
            :rules="[...defaultRule('login'), uniqueLoginRule]"
            required></v-text-field>
            <v-text-field
            v-model="model.password"
            label="Пароль" 
            :rules="[...defaultRule('pass'), passRule]"
            required type="password"></v-text-field>
            <v-text-field
            label="Повторите пароль"
            :rules="confirmPassRule"
            required type="password"></v-text-field>
          </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer/>
            <v-btn @click="sendData()" color="primary" :loading="loading" small>Регистрация</v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>
    </v-layout>   
  </v-container>
</template>

<script>
  import onResize from "../../mixins/on-resize"
  import formValidation from "../../mixins/form-validation"
  export default {
    mixins:[onResize, formValidation],
    data(){
      return{
        loading:false,
        errorsModel:{},
        valid:false,
        model:{
          email:"",
          password:"",
          firstName:"",
          lastname:"",
          userName:"",
          patronymic:""
        },
        rules:{
          login:{first:"Необходимо ввести логин",second:"Логин не должен содержать пробелов"},
          pass:{first:"Необходимо ввести пароль",second:"Пароль не должен содержать пробелов"},
          fname:{first:"Необходимо ввести имя",second:"Имя не должно содержать пробелов"},
          lname:{first:"Необходимо ввести фамилию",second:"Фамилия не должена содержать пробелов"},
          email:{first:"Необходимо ввести email",second:"Email не должен содержать пробелов"},
        },
        confirmPassRule:[v=> v === this.model.password || "Пароли не совпадают"],
        emailRule:v => /.+@.+/.test(v) || 'Неправильный email',
        passRule:v => (v || '').length >= 6 ||
            `Пароль должен быть длиннее 6 символов`,
        uniqueEmailRule:v => !(this.errorsModel.DuplicateEmail == this.model.email) || 'Этот email занят',
        uniqueLoginRule:v=> !(this.errorsModel.DuplicateUserName == this.model.userName) || 'Этот login занят'
      }
    },
    methods:{
      defaultRule(label){
        var e = this.rules[label];
        return [v=>!!v||e.first, v=>!/^.*\s.*$/.test(v) ||e.second];
      },
      sendData(){
        if(!this.valid)
          return;
        this.errorsModel = {};
        this.loading = true;
        this.$http.post(`/api/account/register`, this.model).then(
          r=>{
            this.$router.push({name:"afterRegister"});
            this.loading = false;
          },
          r=>{
            if(r.response.status == 500){
              console.log(r.response);
              this.loading = false;
              return;
            }
            var errors = r.response.data;
            if(errors.some(e=>e.code == 'DuplicateEmail'))
              this.errorsModel.DuplicateEmail = this.model.email;
            if(errors.some(e=>e.code == 'DuplicateUserName'))
              this.errorsModel.DuplicateUserName = this.model.userName
            this.validate();
            this.loading = false;
          }
        )
      }
    }
  }
</script>
