<template>
    <v-dialog v-model="opened" :width="ofSize({xs:350, sm:400})" persistent>
        <v-btn slot="activator">
            <v-icon>add</v-icon>
            <span class="text-none">Создать проект</span>
        </v-btn>
        <v-card class="elevation-12">
            <v-toolbar dark color="primary">
                <v-toolbar-title>Новый проект</v-toolbar-title>
                <v-spacer></v-spacer>
                <v-btn icon @click="close()">
                  <v-icon >
                    close
                  </v-icon>
                </v-btn>
            </v-toolbar>
            <v-card-text>
                <v-form ref="form" v-model="valid">
                    <v-subheader>Название</v-subheader>
                    <v-text-field class="mx-3" 
                    :rules="nameRules" 
                    v-model="name" 
                    required
                    counter="30"/>
                    <v-layout column align-center>
                        <v-subheader>Логотип</v-subheader>
                        <img v-show="logo" :src="logo" height="124px"/>
                        <v-icon v-show="!logo" size=124px>
                            work
                        </v-icon>
                        <input class="mt-2" type="file" @change="onFilePhotoChange"/>
                    </v-layout>
                </v-form>
            </v-card-text>
            <v-card-actions>
                <v-layout row justify-end>
                    <v-btn color="primary" :loading="loading" class="mr-2" @click="submit()">Создать</v-btn>
                </v-layout>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import onResize from '../../mixins/on-resize'
import formValidation from '../../mixins/form-validation'
import { mapActions } from 'vuex'
export default {
    mixins:[onResize, formValidation],
    data(){
        return {
            name:"",
            loading:false,
            valid:true,
            logo:"",
            opened:false,
            nameRules:[v=>v?v.length>2||"Длина названия от 3х символов":"Введите название"]
        }
    },
    methods: {
        submit(){
            this.validate();
            if(this.valid){
                this.$http.post("api/project", {Name:this.name, Logo:this.logo}).then(
                    r=>{this.fetchProjects(); this.close();},
                    r=>{console.log(r.response)}
                );
            }
        },
        onFilePhotoChange($event) {
            const files = $event.target.files || $event.dataTransfer.files;
            if (!files.length) return;
            var fd  = new FormData();
            fd.append('file', files[0]);
            this.$http.post('/api/file/image', fd, { headers: {'Content-Type': 'multipart/form-data' }}).then(
                r=>{this.logo = r.data;},
                r=>{console.log(r.responce);}
            );
        },
        ...mapActions({fetchProjects:'projects/fetchProjects'}),
        close(){
          this.logo = "";
          this.reset();
          this.opened = false;
        }
    }
}
</script>
