<template>
    <v-layout justify-center class="mt-3">
        <v-card width="400">
            <v-card-title>
                <span class="title">Ваш профиль</span>
            </v-card-title>
            <v-container v-show="loading">
                <v-layout column justify-center align-center fill-height>
                    <v-progress-circular indeterminate color="primary"></v-progress-circular>
                </v-layout>
            </v-container>
            <v-card-text v-show="!loading">
                <v-form ref="form">
                    <v-layout justify-center>
                        <image-loader class="pl-4" @fotoLoaded="avatarChanged" :image="profile.avatar">
                            <v-btn icon small class="close" @click="profile.avatar = null">
                                <v-icon small>close</v-icon>
                            </v-btn>
                        </image-loader>
                    </v-layout>
                    <v-text-field
                    v-model="profile.firstName" 
                    label="Имя" 
                    required></v-text-field>
                    <v-text-field
                    v-model="profile.lastName" 
                    label="Фамилия" 
                    required></v-text-field>
                    <v-text-field 
                    v-model="profile.patronymic" 
                    label="Отчество"></v-text-field>
                    <v-text-field disabled
                    v-model="profile.email" 
                    label="Email"></v-text-field>
                </v-form>
            </v-card-text>
            <v-card-actions v-show="!loading">
                <v-spacer/>
                <v-btn class="primary" small @click="close()">Отмена</v-btn>
                <v-btn class="primary" small @click="submit()">Сохранить</v-btn>
            </v-card-actions>
        </v-card>
    </v-layout>
</template>

<script>
import {mapGetters, mapActions, mapMutations} from 'vuex'
import imageLoader from '../image-loader'
export default {
    components:{
        'image-loader':imageLoader
    },
    mounted(){
        this.fetch();
    },
    methods:{
        close(){
            this.$router.go(-1);
        },
        ...mapActions({fetch:'profile/open'}),
        ...mapMutations({signIn:'auth/signIn'}),
        avatarChanged(path){
            this.profile.avatar = path;
        },
        submit(){
            this.$http.put(`/api/account/info`,{
                ...this.profile
            }).then(r=>{
                this.signIn({profile:r.data});
            },r=>{
                console.log(r.response)
            })
        }
    },
    computed:{
        ...mapGetters({
            profile:'profile/profile',
            loading:'profile/loading'
        })
    }
}
</script>

