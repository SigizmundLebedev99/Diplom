<template>
    <v-dialog v-model="dialog" :width="ofSize({xs:300, md:500})" persistent>
        <v-btn small class="text-none" v-show="role == 1" slot="activator">
            Пригласить участника
        </v-btn>
        <v-card>
            <v-toolbar class="primary" dark dense>
                <v-toolbar-title>Добавление участника</v-toolbar-title>
                <v-spacer/>
                <v-btn icon @click="close()">
                    <v-icon>close</v-icon>
                </v-btn>
            </v-toolbar>
            <v-card-text class="pb-0">
                <v-form ref="form" v-model="valid">
                    <v-text-field label="Email" v-model="email" :rules="emailRules"></v-text-field>
                </v-form>
                <p class="text-xs-center mx-3">На этот адрес будет отправлен инвайт, позволяющий присоединиться к данному проекту в качестве участника</p>
            </v-card-text>
            <v-card-actions>
                <v-spacer/>
                <v-btn 
                :loading="loading" 
                small 
                class="primary white--text text-none"
                @click="submit()">
                    Отправить инвайт
                    </v-btn>
            </v-card-actions>
        </v-card>   
    </v-dialog>
</template>

<script>
import {mapGetters} from 'vuex'
import onResize from '../../../mixins/on-resize'
import formValidation from '../../../mixins/form-validation'
export default {
    mixins:[onResize, formValidation],
    data:()=>({
        loading:false,
        dialog:false,
        email:'',
        valid:true,
        emailRules:[v=>!!v||"Введите Email", 
        v=>!/^.*\s.*$/.test(v) ||"Email не должен содержать пробелов",
        v => /.+@.+/.test(v) || 'Неправильный email']
    }),
    methods:{
        close(){
            this.reset();
            this.resetValidation();
            this.dialog = false;
        },
        submit(){
            this.loading = true;
            this.$http.post(`/api/partisipant/invite`,
            {
                email:this.email,
                projectId:this.$route.params.projId
            }).then(
                r=>{
                    this.loading=false;
                    this.close();
                    },
                r=>{this.loading=false;})
        }
    },
    computed:{
        ...mapGetters({role:'project/role'})
    }
}
</script>